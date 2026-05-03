import { Routes } from '@angular/router';

import { authGuard } from '../../core/guards/auth.guard';
import { guestGuard } from '../../core/guards/guest.guard';

export const USER_ROUTES: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./Pages/login/login.component').then(c => c.LoginComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./Pages/dashboard/dashboard.component').then(c => c.DashboardComponent)
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  }
];
