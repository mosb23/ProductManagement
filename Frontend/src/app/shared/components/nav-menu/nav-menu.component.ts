import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../../core/services/auth/auth.service';

interface NavigationItem {
  label: string;
  route: string;
  exact: boolean;
  claims: string[];
  group: 'main' | 'products' | 'users';
}

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.scss'
})
export class NavMenuComponent {
  private readonly authService = inject(AuthService);

  @Output() navigate = new EventEmitter<void>();

  readonly navigationItems: NavigationItem[] = [
    { label: 'Dashboard', route: '/dashboard', exact: true, claims: [], group: 'main' },
    { label: 'Products', route: '/products', exact: true, claims: ['products:view'], group: 'products' },
    { label: 'Add Product', route: '/products/create', exact: true, claims: ['products:create'], group: 'products' },
    { label: 'Status History', route: '/products/status-history', exact: true, claims: ['product-status-histories:view'], group: 'products' },
    { label: 'Users', route: '/users', exact: true, claims: ['users:view'], group: 'users' },
    { label: 'Add User', route: '/users/create', exact: true, claims: ['users:create'], group: 'users' },
    { label: 'Roles', route: '/users/roles', exact: true, claims: ['roles:view'], group: 'users' },
    { label: 'Permissions', route: '/users/permissions', exact: true, claims: ['roles:view'], group: 'users' },
    {label: 'Statistics',route: '/statistics',exact: true,claims: ['statistics:view'],group: 'main'}
  ];

  get mainNavigationItems(): NavigationItem[] {
    return this.visibleNavigationItems.filter(item => item.group === 'main');
  }

  get productNavigationItems(): NavigationItem[] {
    return this.visibleNavigationItems.filter(item => item.group === 'products');
  }

  get userNavigationItems(): NavigationItem[] {
    return this.visibleNavigationItems.filter(item => item.group === 'users');
  }

  private get visibleNavigationItems(): NavigationItem[] {
    return this.navigationItems.filter(item => {
      if (item.claims.length === 0) {
        return true;
      }

      return this.authService.hasAnyClaim(item.claims);
    });
  }

  onNavigate(): void {
    this.navigate.emit();
  }
}
