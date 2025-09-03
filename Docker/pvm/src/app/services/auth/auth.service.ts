import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/loginreq';
import { LoginResponse } from '../../models/resp';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_BASE = 'http://localhost:5000/api';
  private platformId = inject(PLATFORM_ID);

  private tokenSubject = new BehaviorSubject<string | null>(this.getToken());
  public token$ = this.tokenSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.API_BASE}/auth/login`, credentials)
      .pipe(
        tap(response => this.setToken(response.token))
      );
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
      return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
    } catch {
      return null;
    }
  }

  getUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;
    
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.userId || payload.userID || payload.UserId || null;
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
