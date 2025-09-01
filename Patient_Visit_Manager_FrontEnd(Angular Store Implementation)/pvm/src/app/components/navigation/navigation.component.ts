import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { AuthState } from '../../store/state/auth.state';
import { Logout } from '../../store/actions/auth.actions';
import { CheckAuth } from '../../store/actions/auth.actions';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navigation.component.html'
})
export class NavigationComponent implements OnInit {
  userRole$ = this.store.select(AuthState.userRole);
  isAuthenticated$ = this.store.select(AuthState.isAuthenticated);

  constructor(private store: Store) {
    this.store.dispatch(new CheckAuth());
}

 ngOnInit(): void {
 }

  onLogout(): void {
    this.store.dispatch(new Logout());
  }
}