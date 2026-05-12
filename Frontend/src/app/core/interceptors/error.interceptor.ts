import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../services/auth/auth.service';
import { AlertService } from '../services/alert.service';
import { mapHttpError } from '../utils/http-error.util';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const alertService = inject(AlertService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiError = mapHttpError(error);

      if (apiError.status === 401) {
        authService.clearSession();
        router.navigate(['/login']);
        alertService.error(apiError.message);
      } else if (apiError.status === 403) {
        router.navigate(['/forbidden']);
        alertService.error(apiError.message);
      } else if (apiError.status === 500 || apiError.status === 0) {
        alertService.error(apiError.message);
      }

      return throwError(() => apiError);
    })
  );
};
