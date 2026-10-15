import { Routes } from '@angular/router';
import { anonymousOnlyGuard } from '../../core/auth/guards/anonymous-only.guard';

export const authRoutes: Routes = [
  {
    path: 'login',
    canActivate: [anonymousOnlyGuard],
    loadComponent: () => import('./login-page/login-page').then((module) => module.LoginPage),
  },
];
