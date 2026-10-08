import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const logisticsAccessGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  if (!authService.accessToken) {
    return inject(Router).parseUrl('/');
  }

  return authService.hasCategory('C000000005') || inject(Router).parseUrl('/');
};
