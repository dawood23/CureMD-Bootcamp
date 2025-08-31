import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth/auth.guard';
import { RoleGuard } from './guards/role/role.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => 
        import('./Auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'signup',
    loadComponent: () => import('./Auth/signup/signup.component').then(m => m.SignupComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'users',
    loadComponent: () => import('./components/users/users.component').then(m => m.UsersComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: 'user-roles',
    loadComponent: () => import('./components/userroles/userroles.component').then(m => m.UserRolesComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: 'doctors',
    loadComponent: () => import('./components/doctor/doctor.component').then(m => m.DoctorsComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin'] }
  },
  {
    path: 'patients',
    loadComponent: () => import('./components/patients/patients.component').then(m => m.PatientsComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin', 'Doctor', 'Receptionist'] }
  },
  {
    path: 'visits',
    loadComponent: () => import('./components/visits/visits.component').then(m => m.VisitsComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin', 'Doctor', 'Receptionist'] }
  },
  {
    path: 'visit-types',
    loadComponent: () => import('./components/visittypes/visittypes.component').then(m => m.VisittypesComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Admin', 'Receptionist'] }
  },
  { path: '**', redirectTo: '/login' }
];
