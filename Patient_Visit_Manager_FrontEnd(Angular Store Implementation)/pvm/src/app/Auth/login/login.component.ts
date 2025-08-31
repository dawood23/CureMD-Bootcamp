import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';
import { LoginRequest } from '../../models/loginreq';
import { Login } from '../../store/actions/auth.actions';
import { AuthState } from '../../store/state/auth.state';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: "./login.component.html"
})
export class LoginComponent {
  credentials: LoginRequest = {
    username: '',
    password: ''
  };

  loading$ = this.store.select(AuthState.loading);
  error$ = this.store.select(AuthState.error);

  constructor(private store: Store, private router: Router) {}

  onLogin(): void {
    this.store.dispatch(new Login(this.credentials));
  }
}