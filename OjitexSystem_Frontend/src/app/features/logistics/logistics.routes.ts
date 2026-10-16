import { Routes } from '@angular/router';
import { logisticsAccessGuard } from '../../core/auth/guards/category-access.guard';
import { StockReportKind } from './stock-report/stock-report.models';

const reportKinds: StockReportKind[] = ['current', 'last', 'monthly'];

export const logisticsRoutes: Routes = reportKinds.map((kind) => ({
  path: `logistic/${kind}-stock`,
  canActivate: [logisticsAccessGuard],
  data: { stockReport: kind },
  loadComponent: () =>
    import('./stock-report/stock-report-page').then((module) => module.StockReportPage),
}));
