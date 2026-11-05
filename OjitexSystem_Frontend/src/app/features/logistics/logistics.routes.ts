import { Routes } from '@angular/router';
import { logisticsAccessGuard } from '../../core/auth/guards/category-access.guard';
import { StockReportKind } from './stock-report/stock-report.models';

const reportKinds: StockReportKind[] = ['current', 'last', 'monthly'];

export const logisticsRoutes: Routes = [
  ...reportKinds.map((kind) => ({
    path: `logistic/${kind}-stock`,
    canActivate: [logisticsAccessGuard],
    data: { stockReport: kind },
    loadComponent: () =>
      import('./stock-report/stock-report-page').then((module) => module.StockReportPage),
  })),
  {
    path: 'logistic/delivery-note-search',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./delivery-note-search/delivery-note-search-page').then(
        (module) => module.DeliveryNoteSearchPage,
      ),
  },
  {
    path: 'logistic/menu',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./logistics-menu/logistics-menu.component').then(
        (module) => module.LogisticsMenuComponent,
      ),
  },
];
