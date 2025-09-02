import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navigation.component.html'
})
export class NavigationComponent implements OnInit {
  userRole: string | null = null;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.userRole = this.auth.getUserRole();
  }

  onLogout(): void {
    this.auth.logout();
  }
}