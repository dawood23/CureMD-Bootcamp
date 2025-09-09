import { Routes } from '@angular/router';
import { AuthGuard } from './guards/Auth/auth.guard';
import { GuestGuard } from './guards/guest/guest.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'
   },
  { path: 'login', loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent) ,pathMatch:'full',
    canActivate:[GuestGuard]
  },
  {path:'signup',loadComponent:() =>import('./components/auth/signup/signup.component').then(m=>m.SignupComponent),pathMatch:'full',
     canActivate:[GuestGuard]
  },
  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent) ,pathMatch:'full',
    canActivate:[AuthGuard]
  },
   { path: 'patient-list', loadComponent: () => import('./components/patients/patient-list/patient-list.component').then(m => m.PatientListComponent) ,pathMatch:'full',
    canActivate:[AuthGuard]
  },
  { path: 'patient-add', loadComponent: () => import('./components/patients/addpatient/addpatient.component').then(m => m.AddpatientComponent) ,pathMatch:'full',
    canActivate:[AuthGuard]
  },
{
  path:'appointment-list',loadComponent:()=> import('./components/appointments/appointment-list/appointment-list.component').then(m=>m.AppointmentListComponent),pathMatch:'full',
  canActivate:[AuthGuard]
},
{
  path:'appointment-add',loadComponent:()=> import('./components/appointments/create-appointment/create-appointment.component').then(m=>m.CreateAppointmentComponent),pathMatch:'full',
  canActivate:[AuthGuard]
}
  ,
  {path:'**',redirectTo:'login'}
];
