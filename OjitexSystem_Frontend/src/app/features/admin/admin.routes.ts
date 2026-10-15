import { Routes } from '@angular/router';
import { adminAccessGuard } from '../../core/auth/guards/admin-access.guard';

export const adminRoutes: Routes = [
  { path: 'department/admin', pathMatch: 'full', redirectTo: 'admin/dashboard' },
  {
    path: 'admin/dashboard',
    canActivate: [adminAccessGuard],
    loadComponent: () =>
      import('./user-management/admin-dashboard-page').then((module) => module.AdminDashboardPage),
  },
];
