import { Routes } from '@angular/router';

export const navigationRoutes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  {
    path: 'home',
    loadComponent: () => import('./home/home-page').then((module) => module.HomePage),
  },
  {
    path: 'department/:departmentId',
    loadComponent: () =>
      import('./department/department-page').then((module) => module.DepartmentPage),
  },
];
