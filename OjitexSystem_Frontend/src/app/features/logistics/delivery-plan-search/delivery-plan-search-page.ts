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
  DELIVERY_PLAN_SEARCH_COLUMN_DEFS,
  DELIVERY_PLAN_SEARCH_DEFAULT_COLUMN,
} from './delivery-plan-search-columns';
import { DeliveryPlanFilters, DeliveryPlanSearchResult } from './delivery-plan-search.models';
import { DeliveryPlanService } from './delivery-plan.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

const EMPTY_FILTERS: DeliveryPlanFilters = {
  deliveryDate: '',
  receiveNo: '',
};

@Component({
  selector: 'app-delivery-plan-search-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './delivery-plan-search-page.html',
  styleUrl: './delivery-plan-search-page.scss',
})
export class DeliveryPlanSearchPage {
  private readonly service = inject(DeliveryPlanService);
  private readonly searches = new Subject<DeliveryPlanFilters | null>();

  protected readonly filters = signal<DeliveryPlanFilters>({ ...EMPTY_FILTERS });
  protected readonly columns = DELIVERY_PLAN_SEARCH_COLUMN_DEFS;
  protected readonly defaultColDef = DELIVERY_PLAN_SEARCH_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<DeliveryPlanSearchResult | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof DeliveryPlanFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.search(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Delivery Plan Search. Please search again.',
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

  protected changeFilter(field: keyof DeliveryPlanFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const deliveryDate = current.deliveryDate.trim();
    const receiveNo = current.receiveNo.trim();

    if (deliveryDate && !isValidReportDate(deliveryDate)) {
      this.invalidField.set('deliveryDate');
      this.error.set('Enter a valid date in mm/dd/yyyy format.');
      return;
    }
    if (receiveNo.length > 20) {
      this.invalidField.set('receiveNo');
      this.error.set('Receive no must contain at most 20 characters.');
      return;
    }

    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ deliveryDate, receiveNo });
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
