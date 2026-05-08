import { Routes } from '@angular/router';

import { permissionGuard } from '../../core/guards/permission.guard';

export const PRODUCT_ROUTES: Routes = [
  {
    path: '',
    canActivate: [permissionGuard],
    data: { permission: 'products:view' },
    loadComponent: () =>
      import('./Pages/product-list/product-list.component')
        .then(c => c.ProductListComponent)
  },
  {
    path: 'create',
    canActivate: [permissionGuard],
    data: { permission: 'products:create' },
    loadComponent: () =>
      import('./Pages/add-product/add-product.component')
        .then(c => c.AddProductComponent)
  },
  {
    path: 'status-history',
    canActivate: [permissionGuard],
    data: { permission: 'product-status-histories:view' },
    loadComponent: () =>
      import('./Pages/product-status-history-list/product-status-history-list.component')
        .then(c => c.ProductStatusHistoryListComponent)
  },
  {
    path: ':id',
    canActivate: [permissionGuard],
    data: { permission: 'products:view' },
    loadComponent: () =>
      import('./Pages/product-details/product-details.component')
        .then(c => c.ProductDetailsComponent)
  }
];