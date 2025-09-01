import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as UserRoleActions from '../actions/user-role.actions';
import { UserRole } from '../../models/user-roles';
import { UserRoleStateModel } from '../models/user-role.state.model';

@State<UserRoleStateModel>({
  name: 'userRole',
  defaults: {
    userRoles: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class UserRoleState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static userRoles(state: UserRoleStateModel): UserRole[] {
    return state.userRoles;
  }

  @Selector()
  static loading(state: UserRoleStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: UserRoleStateModel): string | null {
    return state.error;
  }

  @Selector()
  static userRoleById(state: UserRoleStateModel) {
    return (id: number) => state.userRoles.find(ur => ur.roleID === id);
  }

  @Selector()
  static totalUserRoles(state: UserRoleStateModel): number {
    return state.userRoles.length;
  }

  @Action(UserRoleActions.LoadUserRoles)
  loadUserRoles(ctx: StateContext<UserRoleStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getUserRoles().pipe(
      tap(userRoles => {
        ctx.dispatch(new UserRoleActions.LoadUserRolesSuccess(userRoles));
      }),
      catchError(error => {
        ctx.dispatch(new UserRoleActions.LoadUserRolesFailure(error.message || 'Failed to load user roles'));
        return of(null);
      })
    );
  }

  @Action(UserRoleActions.LoadUserRolesSuccess)
  loadUserRolesSuccess(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.LoadUserRolesSuccess) {
    ctx.patchState({
      userRoles: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(UserRoleActions.LoadUserRolesFailure)
  loadUserRolesFailure(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.LoadUserRolesFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserRoleActions.CreateUserRole)
  createUserRole(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.CreateUserRole) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createUserRole(action.payload).pipe(
      tap(userRole => {
        ctx.dispatch(new UserRoleActions.CreateUserRoleSuccess(userRole));
        ctx.dispatch(new UserRoleActions.LoadUserRoles());
      }),
      catchError(error => {
        ctx.dispatch(new UserRoleActions.CreateUserRoleFailure(error.message || 'Failed to create user role'));
        return of(null);
      })
    );
  }

  @Action(UserRoleActions.CreateUserRoleSuccess)
  createUserRoleSuccess(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.CreateUserRoleSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      userRoles: [...state.userRoles, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(UserRoleActions.CreateUserRoleFailure)
  createUserRoleFailure(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.CreateUserRoleFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserRoleActions.UpdateUserRole)
  updateUserRole(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.UpdateUserRole) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updateUserRole(action.payload).pipe(
      tap(userRole => {
        ctx.dispatch(new UserRoleActions.UpdateUserRoleSuccess(userRole));
        ctx.dispatch(new UserRoleActions.LoadUserRoles());
      }),
      catchError(error => {
        ctx.dispatch(new UserRoleActions.UpdateUserRoleFailure(error.message || 'Failed to update user role'));
        return of(null);
      })
    );
  }

  @Action(UserRoleActions.UpdateUserRoleSuccess)
  updateUserRoleSuccess(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.UpdateUserRoleSuccess) {
    const state = ctx.getState();
    const updatedUserRoles = state.userRoles.map(ur => 
      ur.roleID === action.payload.roleID ? action.payload : ur
    );
    
    ctx.patchState({
      userRoles: updatedUserRoles,
      loading: false,
      error: null
    });
  }

  @Action(UserRoleActions.UpdateUserRoleFailure)
  updateUserRoleFailure(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.UpdateUserRoleFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserRoleActions.DeleteUserRole)
  deleteUserRole(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.DeleteUserRole) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deleteUserRole(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new UserRoleActions.DeleteUserRoleSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new UserRoleActions.DeleteUserRoleFailure(error.message || 'Failed to delete user role'));
        return of(null);
      })
    );
  }

  @Action(UserRoleActions.DeleteUserRoleSuccess)
  deleteUserRoleSuccess(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.DeleteUserRoleSuccess) {
    const state = ctx.getState();
    const filteredUserRoles = state.userRoles.filter(ur => ur.roleID !== action.payload);
    
    ctx.patchState({
      userRoles: filteredUserRoles,
      loading: false,
      error: null
    });
  }

  @Action(UserRoleActions.DeleteUserRoleFailure)
  deleteUserRoleFailure(ctx: StateContext<UserRoleStateModel>, action: UserRoleActions.DeleteUserRoleFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}