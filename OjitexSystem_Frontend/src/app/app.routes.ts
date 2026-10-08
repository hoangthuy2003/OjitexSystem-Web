import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./views/auth/login-page/login-page').then((module) => module.LoginPage),
  },
  {
    path: 'logistic/current-stock',
    loadComponent: () =>
      import('./views/logistic/current-stock/current-stock-page').then(
        (module) => module.CurrentStockPage,
      ),
  },
  { path: '**', redirectTo: '' },
];
