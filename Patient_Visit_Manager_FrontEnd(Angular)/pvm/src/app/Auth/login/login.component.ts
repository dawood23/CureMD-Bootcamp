import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { LoginRequest } from '../../models/loginreq';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <div class="fl">
      <div class="LoginForm">
        <h2>Login</h2>
        <form (ngSubmit)="onLogin()" #loginForm="ngForm">
          <label>Username:</label><br />
          <input type="text" [(ngModel)]="credentials.username" name="username" required /><br />
          <label>Password:</label><br />
          <input type="password" [(ngModel)]="credentials.password" name="password" required /><br /><br />
          <button type="submit" [disabled]="!loginForm.form.valid">Login</button>
          <p>Don't have an account? <a routerLink="/signup">Signup</a></p>
        </form>
      </div>
    </div>
  `,
})
export class LoginComponent {
  credentials: LoginRequest = {
    username: '',
    password: ''
  };

  constructor(private auth: AuthService, private router: Router) {}

  onLogin(): void {
    this.auth.login(this.credentials).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        alert('Invalid username or password');
      }
    });
  }
}