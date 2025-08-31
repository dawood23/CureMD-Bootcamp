import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { ApiService } from '../../services/api/api.service';
import * as UserActions from '../actions/user.actions';
import { UserStateModel } from '../models/user.state.model';
import { User } from '../../models/user';

@State<UserStateModel>({
  name: 'user',
  defaults: {
    users: [],
    loading: false,
    error: null
  }
})
@Injectable()
export class UserState {
  constructor(private apiService: ApiService) {}

  @Selector()
  static users(state: UserStateModel): User[] {
    return state.users;
  }

  @Selector()
  static loading(state: UserStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: UserStateModel): string | null {
    return state.error;
  }

  @Selector()
  static userById(state: UserStateModel) {
    return (id: number) => state.users.find(u => u.userID === id);
  }

  @Selector()
  static totalUsers(state: UserStateModel): number {
    return state.users.length;
  }

  @Action(UserActions.LoadUsers)
  loadUsers(ctx: StateContext<UserStateModel>) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.getUsers().pipe(
      tap(users => {
        ctx.dispatch(new UserActions.LoadUsersSuccess(users));
      }),
      catchError(error => {
        ctx.dispatch(new UserActions.LoadUsersFailure(error.message || 'Failed to load users'));
        return of(null);
      })
    );
  }

  @Action(UserActions.LoadUsersSuccess)
  loadUsersSuccess(ctx: StateContext<UserStateModel>, action: UserActions.LoadUsersSuccess) {
    ctx.patchState({
      users: action.payload,
      loading: false,
      error: null
    });
  }

  @Action(UserActions.LoadUsersFailure)
  loadUsersFailure(ctx: StateContext<UserStateModel>, action: UserActions.LoadUsersFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserActions.CreateUser)
  createUser(ctx: StateContext<UserStateModel>, action: UserActions.CreateUser) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.createUser(action.payload).pipe(
      tap(user => {
        ctx.dispatch(new UserActions.CreateUserSuccess(user));
      }),
      catchError(error => {
        ctx.dispatch(new UserActions.CreateUserFailure(error.message || 'Failed to create user'));
        return of(null);
      })
    );
  }

  @Action(UserActions.CreateUserSuccess)
  createUserSuccess(ctx: StateContext<UserStateModel>, action: UserActions.CreateUserSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      users: [...state.users, action.payload],
      loading: false,
      error: null
    });
  }

  @Action(UserActions.CreateUserFailure)
  createUserFailure(ctx: StateContext<UserStateModel>, action: UserActions.CreateUserFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserActions.UpdateUser)
  updateUser(ctx: StateContext<UserStateModel>, action: UserActions.UpdateUser) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.updateUser(action.payload.id, action.payload.user).pipe(
      tap(user => {
        ctx.dispatch(new UserActions.UpdateUserSuccess(user));
      }),
      catchError(error => {
        ctx.dispatch(new UserActions.UpdateUserFailure(error.message || 'Failed to update user'));
        return of(null);
      })
    );
  }

  @Action(UserActions.UpdateUserSuccess)
  updateUserSuccess(ctx: StateContext<UserStateModel>, action: UserActions.UpdateUserSuccess) {
    const state = ctx.getState();
    const updatedUsers = state.users.map(u => 
      u.userID === action.payload.userID ? action.payload : u
    );
    
    ctx.patchState({
      users: updatedUsers,
      loading: false,
      error: null
    });
  }

  @Action(UserActions.UpdateUserFailure)
  updateUserFailure(ctx: StateContext<UserStateModel>, action: UserActions.UpdateUserFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(UserActions.DeleteUser)
  deleteUser(ctx: StateContext<UserStateModel>, action: UserActions.DeleteUser) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.deleteUser(action.payload).pipe(
      tap(() => {
        ctx.dispatch(new UserActions.DeleteUserSuccess(action.payload));
      }),
      catchError(error => {
        ctx.dispatch(new UserActions.DeleteUserFailure(error.message || 'Failed to delete user'));
        return of(null);
      })
    );
  }

  @Action(UserActions.DeleteUserSuccess)
  deleteUserSuccess(ctx: StateContext<UserStateModel>, action: UserActions.DeleteUserSuccess) {
    const state = ctx.getState();
    const filteredUsers = state.users.filter(u => u.userID !== action.payload);
    
    ctx.patchState({
      users: filteredUsers,
      loading: false,
      error: null
    });
  }

  @Action(UserActions.DeleteUserFailure)
  deleteUserFailure(ctx: StateContext<UserStateModel>, action: UserActions.DeleteUserFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }
}