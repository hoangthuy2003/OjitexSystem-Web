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
  DELIVERY_MONTHLY_COLUMN_DEFS,
  DELIVERY_MONTHLY_DEFAULT_COLUMN,
} from './delivery-note-monthly-columns';
import { DeliveryMonthlyFilters, DeliveryMonthlyReport } from './delivery-note-monthly.models';
import { DeliveryNoteService } from '../delivery-note-search/delivery-note.service';

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

const EMPTY_FILTERS: DeliveryMonthlyFilters = {
  deliverySlip: '',
  customerCode: '',
  productCode: '',
  fromDate: '',
  toDate: '',
};

@Component({
  selector: 'app-delivery-note-monthly-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './delivery-note-monthly-page.html',
  styleUrl: './delivery-note-monthly-page.scss',
})
export class DeliveryNoteMonthlyPage {
  private readonly service = inject(DeliveryNoteService);
  private readonly searches = new Subject<DeliveryMonthlyFilters | null>();

  private readonly initialDates = getInitialDates();
  protected readonly filters = signal<DeliveryMonthlyFilters>({
    ...EMPTY_FILTERS,
    fromDate: this.initialDates.fromDate,
    toDate: this.initialDates.toDate,
  });
  protected readonly columns = DELIVERY_MONTHLY_COLUMN_DEFS;
  protected readonly defaultColDef = DELIVERY_MONTHLY_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<DeliveryMonthlyReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof DeliveryMonthlyFilters | null>(null);

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
                          'Unable to load Delivery Monthly Information. Please search again.',
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

  protected changeFilter(field: keyof DeliveryMonthlyFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const deliverySlip = current.deliverySlip.trim();
    const customerCode = current.customerCode.trim();
    const productCode = current.productCode.trim();
    const fromDate = current.fromDate.trim();
    const toDate = current.toDate.trim();

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
      const from = new Date(fromDate);
      const to = new Date(toDate);
      if (from > to) {
        this.invalidField.set('fromDate');
        this.error.set('From date must be earlier than or equal to to date.');
        return;
      }
    }
    if (deliverySlip.length > 30) {
      this.invalidField.set('deliverySlip');
      this.error.set('Delivery slip must contain at most 30 characters.');
      return;
    }
    if (customerCode && !/^[0-9]{1,3}$/.test(customerCode)) {
      this.invalidField.set('customerCode');
      this.error.set('Customer code must contain 1 to 3 digits.');
      return;
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
    this.searches.next({ deliverySlip, customerCode, productCode, fromDate, toDate });
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
