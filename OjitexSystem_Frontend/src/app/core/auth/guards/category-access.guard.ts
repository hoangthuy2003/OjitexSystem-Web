import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { LOGISTICS_CATEGORY_ID } from '../access.constants';
import { AuthService } from '../auth.service';

export const logisticsAccessGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  if (!authService.accessToken) {
    return inject(Router).parseUrl('/login');
  }

  return authService.hasCategory(LOGISTICS_CATEGORY_ID) || inject(Router).parseUrl('/home');
};
