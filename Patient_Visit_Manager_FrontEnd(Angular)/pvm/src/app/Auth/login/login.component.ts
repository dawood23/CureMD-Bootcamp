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
  templateUrl:"./login.component.html"
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