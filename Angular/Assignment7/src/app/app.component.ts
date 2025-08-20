import { Component} from '@angular/core';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports:[DashboardComponent,RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone:true
})
export class AppComponent{
  title:string="Assignment#7"
  
}
