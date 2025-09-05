import { Routes } from '@angular/router';
import { AuthGuard } from './guards/Auth/auth.guard';
import { GuestGuard } from './guards/guest/guest.guard';
export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full'
   },
  { path: 'login', loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent) ,
    canActivate:[GuestGuard]
  },
  {path:'signup',loadComponent:() =>import('./components/auth/signup/signup.component').then(m=>m.SignupComponent),
     canActivate:[GuestGuard]
  },
  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent) ,
    canActivate:[AuthGuard]
  },
   { path: 'patient-list', loadComponent: () => import('./components/patients/patient-list/patient-list.component').then(m => m.PatientListComponent) ,
    canActivate:[AuthGuard]
  },
  { path: 'patient-add', loadComponent: () => import('./components/patients/addpatient/addpatient.component').then(m => m.AddpatientComponent) ,
    canActivate:[AuthGuard]
  },
  {path:'**',redirectTo:'login'}
];
