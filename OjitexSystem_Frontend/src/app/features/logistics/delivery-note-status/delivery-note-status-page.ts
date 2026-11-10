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
  DELIVERY_NOTE_STATUS_COLUMN_DEFS,
  DELIVERY_NOTE_STATUS_DEFAULT_COLUMN,
} from './delivery-note-status-columns';
import { DeliveryStatusFilters, DeliveryStatusReport } from './delivery-note-status.models';
import { DeliveryNoteService } from '../delivery-note-search/delivery-note.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

function parseDateForComparison(value: string): number | null {
  if (!isValidReportDate(value)) return null;
  const [month, day, year] = value.split('/').map(Number);
  return new Date(year, month - 1, day).getTime();
}

const EMPTY_FILTERS: DeliveryStatusFilters = {
  fromDate: '',
  toDate: '',
};

@Component({
  selector: 'app-delivery-note-status-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './delivery-note-status-page.html',
  styleUrl: './delivery-note-status-page.scss',
})
export class DeliveryNoteStatusPage {
  private readonly service = inject(DeliveryNoteService);
  private readonly searches = new Subject<DeliveryStatusFilters | null>();

  protected readonly filters = signal<DeliveryStatusFilters>({ ...EMPTY_FILTERS });
  protected readonly columns = DELIVERY_NOTE_STATUS_COLUMN_DEFS;
  protected readonly defaultColDef = DELIVERY_NOTE_STATUS_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<DeliveryStatusReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof DeliveryStatusFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.getStatus(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Delivery Note Status. Please search again.',
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

  protected changeFilter(field: keyof DeliveryStatusFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
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
      const fromTime = parseDateForComparison(fromDate);
      const toTime = parseDateForComparison(toDate);
      if (fromTime !== null && toTime !== null && fromTime > toTime) {
        this.invalidField.set('fromDate');
        this.error.set('From date must be earlier than or equal to to date.');
        return;
      }
    }

    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ fromDate, toDate });
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
