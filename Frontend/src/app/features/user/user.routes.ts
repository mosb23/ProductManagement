import { Routes } from '@angular/router';

import { permissionGuard } from '../../core/guards/permission.guard';

export const USER_ROUTES: Routes = [
  {
    path: '',
    canActivate: [permissionGuard],
    data: { permission: 'users:view' },
    loadComponent: () =>
      import('./Pages/users-list/users-list.component')
        .then(c => c.UsersListComponent)
  },
  {
    path: 'create',
    canActivate: [permissionGuard],
    data: { permission: 'users:create' },
    loadComponent: () =>
      import('./Pages/add-user/add-user.component')
        .then(c => c.AddUserComponent)
  },
  {
    path: 'roles',
    canActivate: [permissionGuard],
    data: { permission: 'roles:view' },
    loadComponent: () =>
      import('./Pages/roles-list/roles-list.component')
        .then(c => c.RolesListComponent)
  },
  {
    path: 'permissions',
    canActivate: [permissionGuard],
    data: { permission: 'roles:view' },
    loadComponent: () =>
      import('./Pages/permissions-list/permissions-list.component')
        .then(c => c.PermissionsListComponent)
  },
  {
    path: ':id',
    canActivate: [permissionGuard],
    data: { permission: 'users:view' },
    loadComponent: () =>
      import('./Pages/user-details/user-details.component')
        .then(c => c.UserDetailsComponent)
  }
];