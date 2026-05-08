import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { permissionGuard } from './core/guards/permission.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () =>
      import('./features/user/Pages/login/login.component')
        .then(c => c.LoginComponent)
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./shared/components/admin-layout/admin-layout.component')
        .then(c => c.AdminLayoutComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/user/Pages/dashboard/dashboard.component')
            .then(c => c.DashboardComponent)
      },
      {
        path: 'products',
        loadChildren: () =>
          import('./features/product/product.routes')
            .then(r => r.PRODUCT_ROUTES)
      },
      {
        path: 'users',
        canActivate: [authGuard],
        loadChildren: () =>
          import('./features/user/user.routes')
            .then(r => r.USER_ROUTES)
      },
      {
        path: 'forbidden',
        loadComponent: () =>
          import('./shared/pages/forbidden/forbidden.component')
            .then(c => c.ForbiddenComponent)
      },
      {
        path: 'statistics',
        canActivate: [permissionGuard],
        data: { permission: 'statistics:view' },
        loadComponent: () =>
          import('./features/statistics/pages/statistics.component')
            .then(c => c.StatisticsComponent)
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'dashboard'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
