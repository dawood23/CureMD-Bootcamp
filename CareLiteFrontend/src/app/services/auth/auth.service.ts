import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { LoginRequest } from '../../models/loginRequest';
import { LoginResponse } from '../../models/loginResponse';
import { URL } from '../../Environment/env';
import { User } from '../../models/user';
import { switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private platformId = inject(PLATFORM_ID);

  private tokenSubject = new BehaviorSubject<string | null>(this.getToken());
  public token$ = this.tokenSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${URL.API_BASE}/auth/login`, credentials)
      .pipe(
        tap(response => {
          if (response.success && response.token) {
            this.setToken(response.token);
          }
        })
      );
  }


signup(credentials: User) {
  return this.http.post<User>(`${URL.API_BASE}/auth/create`, credentials)
}


  logout(): void {
    this.clearToken();
    this.router.navigate(['/login']);
  }

  private setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('jwtToken', token);
    }
    this.tokenSubject.next(token);
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('jwtToken');
    }
    return null;
  }

  private clearToken(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('jwtToken');
    }
    this.tokenSubject.next(null);
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["role"] || null;
    } catch {
      return null;
    }
  }

  getUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return Number(payload["userId"]) || null;
    } catch {
      return null;
    }
  }

  getUsername(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload["name"] || null;
    } catch {
      return null;
    }
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  hasRole(roles: string[]): boolean {
    const userRole = this.getUserRole();
    return userRole ? roles.includes(userRole) : false;
  }
}
