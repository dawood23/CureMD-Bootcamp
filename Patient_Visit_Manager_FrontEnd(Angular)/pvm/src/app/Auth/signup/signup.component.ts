import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api/api.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <header>
      <h1>Sign Up</h1>
    </header>

    <div class="container auth-container">
      <form (ngSubmit)="onSignup()" #signupForm="ngForm">
        <label>Username:</label>
        <input type="text" [(ngModel)]="user.username" name="username" required>

        <label>Password:</label>
        <input type="password" [(ngModel)]="password" name="password" required>

        <label>Role:</label>
        <select [(ngModel)]="user.roleID" name="roleID" required>
          <option value="">-- Select a Role --</option>
          <option value="1">Admin</option>
          <option value="2">Receptionist</option>
          <option value="3">Doctor</option>
        </select>

        <label>First Name:</label>
        <input type="text" [(ngModel)]="user.firstName" name="firstName" required>

        <label>Last Name:</label>
        <input type="text" [(ngModel)]="user.lastName" name="lastName" required>

        <button type="submit" class="btn btn-primary" [disabled]="!signupForm.form.valid">Sign Up</button>
        <p>Already have an account? <a routerLink="/login">Login</a></p>
      </form>
    </div>
  `,
})
export class SignupComponent {
  user: User = {
    username: '',
    roleID: 0,
    firstName: '',
    lastName: ''
  };
  password: string = '';

  constructor(private api: ApiService, private router: Router) {}

  onSignup(): void {
    const userData = { ...this.user, passwordHash: this.password };
    
    this.api.createUser(userData).subscribe({
      next: () => {
        alert('Signup successful! Please log in.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        if (error.status === 409) {
          alert('Username already exists.');
        } else {
          alert('Error signing up.');
        }
      }
    });
  }
}