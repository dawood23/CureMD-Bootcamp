import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from "@angular/router";
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit{
  role:any
  authService=inject(AuthService)
  
  ngOnInit(): void {
      this.role=this.authService.getUserRole()
  }

  logout(){
    this.authService.logout();
  }
}
