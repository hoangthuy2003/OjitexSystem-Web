import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../auth.service';

export const authenticatedGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  return !!authService.accessToken || inject(Router).parseUrl('/login');
};
