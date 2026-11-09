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
    path: 'logistic/delivery-note-info',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./delivery-information/delivery-information-page').then(
        (module) => module.DeliveryInformationPage,
      ),
  },
  {
    path: 'logistic/delivery-note-search',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./delivery-note-search/delivery-note-search-page').then(
        (module) => module.DeliveryNoteSearchPage,
      ),
  },
  {
    path: 'logistic/goods-receipt-search',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./goods-receipt-search/goods-receipt-search-page').then(
        (module) => module.GoodsReceiptSearchPage,
      ),
  },
  {
    path: 'logistic/delivery-note-monthly',
    canActivate: [logisticsAccessGuard],
    loadComponent: () =>
      import('./delivery-note-monthly/delivery-note-monthly-page').then(
        (module) => module.DeliveryNoteMonthlyPage,
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
