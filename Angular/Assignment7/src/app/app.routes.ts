import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactPageComponent } from './contact-page/contact-page.component';

export const routes: Routes = [
    {path:"",redirectTo:"/dashboard",pathMatch:'full'},
    {path:"dashboard",component:DashboardComponent,pathMatch:'full'},
    {path:"contacts",children:[
        {path:"",redirectTo:"/contacts/new",pathMatch:'full'},
        {path:"new",component:ContactPageComponent,pathMatch:'full'}
    ]},
    {path:"**",redirectTo:"/"}
];
