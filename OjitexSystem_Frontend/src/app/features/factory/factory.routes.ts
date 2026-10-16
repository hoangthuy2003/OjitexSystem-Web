import { Routes } from '@angular/router';

export const factoryRoutes: Routes = [
  {
    path: 'factory/corrugator-online',
    loadComponent: () =>
      import('./corrugator-online/corrugator-online-page').then(
        (module) => module.CorrugatorOnlinePage,
      ),
  },
];
