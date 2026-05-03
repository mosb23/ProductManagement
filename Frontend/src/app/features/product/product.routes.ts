import { Routes } from '@angular/router';

import { authGuard } from '../../core/guards/auth.guard';

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
    path: ':id',
    loadComponent: () =>
      import('./Pages/product-details/product-details.component')
        .then(c => c.ProductDetailsComponent)
  }
];