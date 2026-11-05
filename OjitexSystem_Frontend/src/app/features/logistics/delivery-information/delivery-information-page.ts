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
  DeliveryInformationFilters,
  DeliveryInformationReport,
} from '../delivery-note-search/delivery-note-search.models';
import { DeliveryNoteService } from '../delivery-note-search/delivery-note.service';
import {
  DELIVERY_INFORMATION_COLUMN_DEFS,
  DELIVERY_INFORMATION_DEFAULT_COLUMN,
} from './delivery-information-columns';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

const EMPTY_FILTERS: DeliveryInformationFilters = {
  deliverySlip: '',
  customerCode: '',
  productCode: '',
  deliveryDate: '',
};

@Component({
  selector: 'app-delivery-information-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './delivery-information-page.html',
  styleUrl: './delivery-information-page.scss',
})
export class DeliveryInformationPage {
  private readonly service = inject(DeliveryNoteService);
  private readonly searches = new Subject<DeliveryInformationFilters | null>();

  protected readonly filters = signal<DeliveryInformationFilters>({ ...EMPTY_FILTERS });
  protected readonly columns = DELIVERY_INFORMATION_COLUMN_DEFS;
  protected readonly defaultColDef = DELIVERY_INFORMATION_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<DeliveryInformationReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof DeliveryInformationFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.getInformation(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Delivery Information. Please search again.',
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

  protected changeFilter(field: keyof DeliveryInformationFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const deliverySlip = current.deliverySlip.trim();
    const customerCode = current.customerCode.trim();
    const productCode = current.productCode.trim();
    const deliveryDate = current.deliveryDate.trim();

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
    if (deliveryDate && !isValidReportDate(deliveryDate)) {
      this.invalidField.set('deliveryDate');
      this.error.set('Enter a valid delivery date in mm/dd/yyyy format.');
      return;
    }

    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ deliverySlip, customerCode, productCode, deliveryDate });
  }

  protected showAll(): void {
    if (this.loading()) return;
    this.filters.set({ ...EMPTY_FILTERS });
    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ ...EMPTY_FILTERS });
  }

  protected clear(): void {
    this.resetResults();
    this.filters.set({ ...EMPTY_FILTERS });
  }

  private resetResults(): void {
    this.searches.next(null);
    this.report.set(null);
    this.error.set(null);
    this.invalidField.set(null);
    this.loading.set(false);
  }
}
