import { Routes } from '@angular/router';
import { adminAccessGuard } from './guards/admin-access.guard';
import { logisticsAccessGuard } from './guards/category-access.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/auth/login-page/login-page').then((module) => module.LoginPage),
  },
  {
    path: 'logistic/current-stock',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./views/logistic/current-stock/current-stock-page').then(
        (module) => module.CurrentStockPage,
      ),
  },
  {
    path: 'admin/users',
    canActivate: [adminAccessGuard],
    loadComponent: () =>
      import('./views/admin/users/admin-users-page').then((module) => module.AdminUsersPage),
  },
  { path: '**', redirectTo: '' },
];
