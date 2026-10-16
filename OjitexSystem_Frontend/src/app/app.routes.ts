import { Routes } from '@angular/router';
import { authenticatedGuard } from './core/auth/guards/authenticated.guard';
import { authRoutes } from './features/auth/auth.routes';
import { adminRoutes } from './features/admin/admin.routes';
import { logisticsRoutes } from './features/logistics/logistics.routes';
import { navigationRoutes } from './features/navigation/navigation.routes';
import { factoryRoutes } from './features/factory/factory.routes';

export const routes: Routes = [
  ...authRoutes,
  {
    path: '',
    canActivate: [authenticatedGuard],
    canActivateChild: [authenticatedGuard],
    children: [...adminRoutes, ...navigationRoutes, ...logisticsRoutes, ...factoryRoutes],
  },
  { path: '**', redirectTo: 'login' },
];
