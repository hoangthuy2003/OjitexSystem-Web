import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const auth = inject(AuthService);
  const token = auth.accessToken;
  if (!request.url.startsWith('/api/') || request.url.split('?')[0] === '/api/auth/login') {
    return next(request);
  }

  const router = inject(Router);
  const authenticatedRequest = token
    ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : request;
  return next(authenticatedRequest).pipe(
    catchError((error: unknown) => {
      // Ignore a rejected token if a newer login has already replaced it.
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const currentToken = auth.accessToken;
        if (currentToken === token || currentToken === null) {
          auth.clearSession();
          void router.navigateByUrl('/login', { replaceUrl: true });
        }
      }
      return throwError(() => error);
    }),
  );
};
