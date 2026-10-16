import { Routes } from '@angular/router';

export const factoryRoutes: Routes = [
  {
    path: 'factory/fps-online',
    loadComponent: () =>
      import('./fps-online/fps-online-page').then((module) => module.FpsOnlinePage),
  },
  {
    path: 'factory/fap-online',
    loadComponent: () =>
      import('./fap-online/fap-online-page').then((module) => module.FapOnlinePage),
  },
  {
    path: 'factory/fg-online',
    loadComponent: () => import('./fg-online/fg-online-page').then((module) => module.FgOnlinePage),
  },
  {
    path: 'factory/evol-online',
    loadComponent: () =>
      import('./evol-online/evol-online-page').then((module) => module.EvolOnlinePage),
  },
  {
    path: 'factory/fdc-online',
    loadComponent: () =>
      import('./fdc-online/fdc-online-page').then((module) => module.FdcOnlinePage),
  },
  {
    path: 'factory/corrugator-online',
    loadComponent: () =>
      import('./corrugator-online/corrugator-online-page').then(
        (module) => module.CorrugatorOnlinePage,
      ),
  },
];
