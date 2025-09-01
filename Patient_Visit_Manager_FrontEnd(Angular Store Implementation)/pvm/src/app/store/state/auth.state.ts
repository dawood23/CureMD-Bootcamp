import { Injectable } from '@angular/core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { tap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { AuthStateModel } from '../models/auth.state.model';
import * as AuthActions from '../actions/auth.actions';

@State<AuthStateModel>({
  name: 'auth',
  defaults: {
    token: null,
    isAuthenticated: false,
    currentUser: null,
    userRole: null,
    userId: null,
    loading: false,
    error: null
  }
})
@Injectable()
export class AuthState {
  constructor(private apiService: AuthService, private router: Router) {}

  @Selector()
  static token(state: AuthStateModel): string | null {
    return state.token;
  }

  @Selector()
  static isAuthenticated(state: AuthStateModel): boolean {
    return state.isAuthenticated;
  }

  @Selector()
  static currentUser(state: AuthStateModel): any | null {
    return state.currentUser;
  }

  @Selector()
  static userRole(state: AuthStateModel): string | null {
    return state.userRole;
  }

  @Selector()
  static userId(state: AuthStateModel): number | null {
    return state.userId;
  }

  @Selector()
  static loading(state: AuthStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static error(state: AuthStateModel): string | null {
    return state.error;
  }

  @Action(AuthActions.Login)
  login(ctx: StateContext<AuthStateModel>, action: AuthActions.Login) {
    ctx.patchState({ loading: true, error: null });
    
    return this.apiService.login(action.payload).pipe(
      tap(response => {
        ctx.dispatch(new AuthActions.LoginSuccess({ token: response.token, user: response }));
        ctx.dispatch(new AuthActions.CheckAuth());
      }),
      catchError(error => {
        ctx.dispatch(new AuthActions.LoginFailure(error.message || 'Login failed'));
        return of(null);
      })
    );
  }

  @Action(AuthActions.LoginSuccess)
  loginSuccess(ctx: StateContext<AuthStateModel>, action: AuthActions.LoginSuccess) {
    const { token } = action.payload;
    
    const payload = JSON.parse(atob(token.split('.')[1]));
    const userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
    const userId = payload.userId || payload.userID || payload.UserId || null;

    ctx.patchState({
      token,
      isAuthenticated: true,
      currentUser: action.payload.user,
      userRole,
      userId,
      loading: false,
      error: null
    });

    if (typeof window !== 'undefined') {
      localStorage.setItem('jwtToken', token);
    }

    this.router.navigate(['/dashboard']);
  }

  @Action(AuthActions.LoginFailure)
  loginFailure(ctx: StateContext<AuthStateModel>, action: AuthActions.LoginFailure) {
    ctx.patchState({
      loading: false,
      error: action.payload
    });
  }

  @Action(AuthActions.Logout)
  logout(ctx: StateContext<AuthStateModel>) {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('jwtToken');
    }
    
    ctx.setState({
      token: null,
      isAuthenticated: false,
      currentUser: null,
      userRole: null,
      userId: null,
      loading: false,
      error: null
    });

    this.router.navigate(['/login']);
  }

  @Action(AuthActions.CheckAuth)
  checkAuth(ctx: StateContext<AuthStateModel>) {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('jwtToken');
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          const userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
          const userId = payload.userId || payload.userID || payload.UserId || null;

          ctx.patchState({
            token,
            isAuthenticated: true,
            userRole,
            userId
          });
        } catch {
          ctx.dispatch(new AuthActions.Logout());
        }
      }
    }
  }
}
