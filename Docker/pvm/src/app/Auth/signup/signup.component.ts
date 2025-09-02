import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api/api.service';
import { User } from '../../models/user';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule,LetterOnlyDirective],
  templateUrl:"./signup.component.html"
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