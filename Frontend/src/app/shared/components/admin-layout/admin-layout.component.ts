import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';

import { AuthService } from '../../../core/services/auth/auth.service';
import { NavMenuComponent } from '../../../shared/components/nav-menu/nav-menu.component';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavMenuComponent],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  private readonly router = inject(Router);
  readonly authService = inject(AuthService);

  isSidebarOpen = false;


  get pageTitle(): string {
    const url = this.router.url.split('?')[0];

    if (url === '/dashboard') return 'Dashboard';
    if (url === '/products') return 'Products';
    if (url === '/products/create') return 'Add Product';
    if (url === '/products/status-history') return 'Status History';
    if (url.startsWith('/products/')) return 'Product Details';

    if (url === '/users') return 'Users';
    if (url === '/users/create') return 'Add User';
    if (url === '/users/roles') return 'Roles';
    if (url === '/users/permissions') return 'Permissions';
    if (url.startsWith('/users/')) return 'User Details';

    if (url === '/forbidden') return 'Access Denied';

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
