import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthService } from '../services/auth/auth.service';

export const permissionGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const requiredPermission = route.data['permission'] as string | undefined;

  if (!authService.isLoggedIn()) {
    authService.clearSession();
    router.navigate(['/login']);
    return false;
  }

  if (!requiredPermission) {
    return true;
  }

  if (authService.hasClaim(requiredPermission)) {
    return true;
  }

  router.navigate(['/forbidden']);
  return false;
};