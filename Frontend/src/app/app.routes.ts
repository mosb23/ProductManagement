import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./features/user/login/Pages/login.component')
        .then(c => c.LoginComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/user/dashboard/Pages/dashboard.component')
        .then(c => c.DashboardComponent)
  },
  {
    path: 'products',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/product/product.routes')
        .then(r => r.productRoutes)
  },
  {
    path: 'forbidden',
    loadComponent: () =>
      import('./shared/pages/forbidden/forbidden.component')
        .then(c => c.ForbiddenComponent)
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'dashboard'
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];