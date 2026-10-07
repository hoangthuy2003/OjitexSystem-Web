import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./views/logistic/current-stock/current-stock-page').then(
        (module) => module.CurrentStockPage,
      ),
  },
];
