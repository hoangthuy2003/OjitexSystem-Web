import { Routes } from '@angular/router';
import { authenticatedGuard } from './guards/authenticated.guard';
import { anonymousOnlyGuard } from './guards/anonymous-only.guard';
import { adminAccessGuard } from './guards/admin-access.guard';
import { logisticsAccessGuard } from './guards/category-access.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [anonymousOnlyGuard],
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
        path: 'department/admin',
        pathMatch: 'full',
        redirectTo: 'admin/dashboard',
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
        path: 'admin/dashboard',
        canActivate: [adminAccessGuard],
        loadComponent: () =>
          import('./views/admin/dashboard/admin-dashboard-page').then(
            (module) => module.AdminDashboardPage,
          ),
      },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
