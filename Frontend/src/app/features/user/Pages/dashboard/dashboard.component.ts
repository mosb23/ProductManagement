import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../../../core/services/auth/auth.service';
import { HasPermissionDirective } from '../../../../shared/directives/has-permission.directive';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, HasPermissionDirective],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  readonly authService = inject(AuthService);

  canViewProducts(): boolean {
    return this.authService.hasClaim('products:view');
  }

  canCreateProducts(): boolean {
    return this.authService.hasClaim('products:create');
  }

  canViewStatusHistory(): boolean {
    return this.authService.hasClaim('product-status-histories:view');
  }

  canViewUsers(): boolean {
    return this.authService.hasClaim('users:view');
  }

  canCreateUsers(): boolean {
    return this.authService.hasClaim('users:create');
  }

  canViewRoles(): boolean {
    return this.authService.hasClaim('roles:view');
  }

  canViewProductSection(): boolean {
    return this.canViewProducts() || this.canCreateProducts() || this.canViewStatusHistory();
  }

  canViewUserSection(): boolean {
    return this.canViewUsers() || this.canCreateUsers() || this.canViewRoles();
  }
}
