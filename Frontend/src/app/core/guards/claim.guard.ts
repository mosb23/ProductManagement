import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

import { AuthService } from '../services/auth/auth.service';

export const claimGuard: CanActivateFn = route => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const claim = route.data?.['claim'] as string | undefined;

  if (!authService.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  if (!claim || authService.hasClaim(claim)) {
    return true;
  }

  router.navigate(['/forbidden']);
  return false;
};
