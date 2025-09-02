import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { NavigationComponent } from '../navigation/navigation.component';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, NavigationComponent],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  userRole: string | null = null;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.userRole = this.auth.getUserRole();
  }

  onLogout(): void {
    this.auth.logout();
  }
}