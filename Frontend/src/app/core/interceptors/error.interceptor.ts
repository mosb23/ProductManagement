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
        alertService.error(apiError.message);
        router.navigate(['/login']);
      } else if (apiError.status === 403) {
        alertService.error(apiError.message);
        router.navigate(['/forbidden']);
      } else if (apiError.status === 500 || apiError.status === 0) {
        alertService.error(apiError.message);
      }

      return throwError(() => apiError);
    })
  );
};