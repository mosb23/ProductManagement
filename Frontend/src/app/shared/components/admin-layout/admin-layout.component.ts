import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthService } from '../../../core/services/auth/auth.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  isSidebarOpen = false;

  readonly navigationItems = [
    { label: 'Dashboard', route: '/dashboard', exact: true },
    { label: 'Products', route: '/products', exact: true },
    { label: 'Add Product', route: '/products/create', exact: true },
    { label: 'Status History', route: '/products/status-history', exact: true, claim: 'product-status-histories:view' }
  ];

  get visibleNavigationItems() {
    return this.navigationItems.filter(item => !item.claim || this.authService.hasClaim(item.claim));
  }

  get pageTitle(): string {
    const url = this.router.url.split('?')[0];

    if (url === '/dashboard') {
      return 'Dashboard';
    }

    if (url === '/products') {
      return 'Products';
    }

    if (url === '/products/create') {
      return 'Add Product';
    }

    if (url === '/products/status-history') {
      return 'Status History';
    }

    if (url.startsWith('/products/')) {
      return 'Product Details';
    }

    if (url === '/forbidden') {
      return 'Access Denied';
    }

    return 'Product Management';
  }

  closeSidebar(): void {
    this.isSidebarOpen = false;
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  logout(): void {
    this.authService.logout();
  }
}
