import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminAccessGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  if (!authService.accessToken) {
    return inject(Router).parseUrl('/');
  }

  return authService.isAdmin || inject(Router).parseUrl('/');
};
