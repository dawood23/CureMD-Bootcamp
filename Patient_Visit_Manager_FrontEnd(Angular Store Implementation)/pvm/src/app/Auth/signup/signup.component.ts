import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { LetterOnlyDirective } from '../../directive/letter-only.directive';
import { CreateUser } from '../../store/actions/user.actions';
import { UserState } from '../../store/state/user.state';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, LetterOnlyDirective],
  templateUrl: "./signup.component.html"
})
export class SignupComponent {
  user: User = {
    username: '',
    roleID: 0,
    firstName: '',
    lastName: ''
  };
  password: string = '';

  loading$ = this.store.select(UserState.loading);
  error$ = this.store.select(UserState.error);

  constructor(private store: Store, private router: Router) {}

  onSignup(): void {
    const userData = { ...this.user, passwordHash: this.password };
    
    this.store.dispatch(new CreateUser(userData)).subscribe({
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
