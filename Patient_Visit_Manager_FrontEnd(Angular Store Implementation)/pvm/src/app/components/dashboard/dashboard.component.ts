import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { NavigationComponent } from '../navigation/navigation.component';
import { AuthState } from '../../store/state/auth.state';
import { Logout,CheckAuth } from '../../store/actions/auth.actions';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, NavigationComponent],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  userRole$ = this.store.select(AuthState.userRole);
  isAuthenticated$ = this.store.select(AuthState.isAuthenticated);

  constructor(private store: Store) {
    this.store.dispatch(new CheckAuth())
  }

  ngOnInit(): void {
    this.store.dispatch(new CheckAuth());
  }

  onLogout(): void {
    this.store.dispatch(new Logout());
  }
}
