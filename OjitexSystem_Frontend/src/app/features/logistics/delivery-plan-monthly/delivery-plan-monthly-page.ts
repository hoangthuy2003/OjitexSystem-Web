import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  ModuleRegistry,
  PaginationModule,
  TooltipModule,
  themeQuartz,
} from 'ag-grid-community';
import { EMPTY, Subject, catchError, switchMap } from 'rxjs';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { ReportClipboardDirective } from '../../../shared/report-clipboard/report-clipboard.directive';
import { isValidReportDate } from '../../factory/report-date';
import {
  DELIVERY_PLAN_MONTHLY_COLUMN_DEFS,
  DELIVERY_PLAN_MONTHLY_DEFAULT_COLUMN,
} from './delivery-plan-monthly-columns';
import {
  DeliveryPlanMonthlyFilters,
  DeliveryPlanMonthlyReport,
} from './delivery-plan-monthly.models';
import { DeliveryPlanService } from '../delivery-plan-search/delivery-plan.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

function getInitialDates(): { fromDate: string; toDate: string } {
  const now = new Date();
  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, '0');
  const day = String(now.getDate()).padStart(2, '0');
  return {
    fromDate: `${month}/01/${year}`,
    toDate: `${month}/${day}/${year}`,
  };
}

function parseDateForComparison(value: string): number | null {
  if (!isValidReportDate(value)) return null;
  const [month, day, year] = value.split('/').map(Number);
  return new Date(year, month - 1, day).getTime();
}

const EMPTY_FILTERS: DeliveryPlanMonthlyFilters = {
  fromDate: '',
  toDate: '',
  productCode: '',
};

@Component({
  selector: 'app-delivery-plan-monthly-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './delivery-plan-monthly-page.html',
  styleUrl: './delivery-plan-monthly-page.scss',
})
export class DeliveryPlanMonthlyPage {
  private readonly service = inject(DeliveryPlanService);
  private readonly searches = new Subject<DeliveryPlanMonthlyFilters | null>();

  private readonly initialDates = getInitialDates();
  protected readonly filters = signal<DeliveryPlanMonthlyFilters>({
    ...EMPTY_FILTERS,
    fromDate: this.initialDates.fromDate,
    toDate: this.initialDates.toDate,
  });
  protected readonly columns = DELIVERY_PLAN_MONTHLY_COLUMN_DEFS;
  protected readonly defaultColDef = DELIVERY_PLAN_MONTHLY_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<DeliveryPlanMonthlyReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof DeliveryPlanMonthlyFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.getMonthly(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Delivery Plan Monthly Information. Please search again.',
                        ),
                  );
                  return EMPTY;
                }),
              ),
        ),
        takeUntilDestroyed(),
      )
      .subscribe((report) => {
        this.report.set(report);
        this.loading.set(false);
      });
  }

  protected changeFilter(field: keyof DeliveryPlanMonthlyFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const fromDate = current.fromDate.trim();
    const toDate = current.toDate.trim();
    const productCode = current.productCode.trim();

    if (fromDate && !isValidReportDate(fromDate)) {
      this.invalidField.set('fromDate');
      this.error.set('Enter a valid from date in mm/dd/yyyy format.');
      return;
    }
    if (toDate && !isValidReportDate(toDate)) {
      this.invalidField.set('toDate');
      this.error.set('Enter a valid to date in mm/dd/yyyy format.');
      return;
    }
    if (fromDate && toDate) {
      const fromTime = parseDateForComparison(fromDate);
      const toTime = parseDateForComparison(toDate);
      if (fromTime !== null && toTime !== null && fromTime > toTime) {
        this.invalidField.set('fromDate');
        this.error.set('From date must be earlier than or equal to to date.');
        return;
      }
    }
    if (productCode && !/^[0-9]{1,7}$/.test(productCode)) {
      this.invalidField.set('productCode');
      this.error.set('Product code must contain 1 to 7 digits.');
      return;
    }

    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ fromDate, toDate, productCode });
  }

  protected clear(): void {
    this.resetResults();
    this.filters.set({
      ...EMPTY_FILTERS,
      fromDate: this.initialDates.fromDate,
      toDate: this.initialDates.toDate,
    });
  }

  private resetResults(): void {
    this.searches.next(null);
    this.report.set(null);
    this.error.set(null);
    this.invalidField.set(null);
    this.loading.set(false);
  }
}
