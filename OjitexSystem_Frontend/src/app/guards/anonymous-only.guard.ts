import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const anonymousOnlyGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  return authService.accessToken ? inject(Router).parseUrl('/home') : true;
};
