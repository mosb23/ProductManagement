import { Routes } from '@angular/router';

import { claimGuard } from '../../core/guards/claim.guard';

export const productRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./Pages/product-list/product-list.component')
        .then(c => c.ProductListComponent)
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./Pages/add-product/add-product.component')
        .then(c => c.AddProductComponent)
  },
  {
    path: 'status-history',
    canActivate: [claimGuard],
    data: { claim: 'product-status-histories:view' },
    loadComponent: () =>
      import('./Pages/product-status-history-list/product-status-history-list.component')
        .then(c => c.ProductStatusHistoryListComponent)
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./Pages/product-details/product-details.component')
        .then(c => c.ProductDetailsComponent)
  }
];
