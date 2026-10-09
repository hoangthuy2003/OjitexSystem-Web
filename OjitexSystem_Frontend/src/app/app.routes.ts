import { Routes } from '@angular/router';
import { authenticatedGuard } from './guards/authenticated.guard';
import { adminAccessGuard } from './guards/admin-access.guard';
import { logisticsAccessGuard } from './guards/category-access.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./views/auth/login-page/login-page').then((module) => module.LoginPage),
  },
  {
    path: '',
    canActivate: [authenticatedGuard],
    canActivateChild: [authenticatedGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'home' },
      {
        path: 'home',
        loadComponent: () =>
          import('./views/home/home-page').then((module) => module.HomePage),
      },
      {
        path: 'department/:departmentId',
        loadComponent: () =>
          import('./views/department/department-page').then((module) => module.DepartmentPage),
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
    ],
  },
  { path: '**', redirectTo: 'login' },
];
