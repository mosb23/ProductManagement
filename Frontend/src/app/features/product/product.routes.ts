import { Routes } from '@angular/router';

import { authGuard } from '../../core/guards/auth.guard';

export const productRoutes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./product-list/Pages/product-list.component')
        .then(c => c.ProductListComponent)
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./add-product/Pages/add-product.component')
        .then(c => c.AddProductComponent)
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./product-details/Pages/product-details.component')
        .then(c => c.ProductDetailsComponent)
  }
];