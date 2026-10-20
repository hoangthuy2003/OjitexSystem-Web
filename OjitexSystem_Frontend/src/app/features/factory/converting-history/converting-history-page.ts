import { DatePipe } from '@angular/common';
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
import { isValidReportDate } from '../report-date';
import {
  CONVERTING_HISTORY_COLUMN_DEFS,
  CONVERTING_HISTORY_DEFAULT_COLUMN,
} from './converting-history-columns';
import { ConvertingHistoryFilters, ConvertingHistoryReport } from './converting-history.models';
import { ConvertingHistoryService } from './converting-history.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

@Component({
  selector: 'app-converting-history-page',
  imports: [DatePipe, FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './converting-history-page.html',
  styleUrl: './converting-history-page.scss',
})
export class ConvertingHistoryPage {
  private readonly service = inject(ConvertingHistoryService);
  private readonly searches = new Subject<ConvertingHistoryFilters | null>();
  protected readonly filters = signal<ConvertingHistoryFilters>({
    date: '',
    process: '',
    productCode: '',
  });
  protected readonly columns = CONVERTING_HISTORY_COLUMN_DEFS;
  protected readonly defaultColDef = CONVERTING_HISTORY_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<ConvertingHistoryReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof ConvertingHistoryFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.getHistory(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Converting History. Please search again.',
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

  protected changeFilter(field: keyof ConvertingHistoryFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const filters = this.filters();
    const date = filters.date.trim();
    const process = filters.process.trim();
    const productCode = filters.productCode.trim();
    if (date && !isValidReportDate(date)) {
      this.invalidField.set('date');
      this.error.set('Enter a valid date in mm/dd/yyyy format.');
      return;
    }
    if (process.length > 6) {
      this.invalidField.set('process');
      this.error.set('Process must be at most 6 characters.');
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
    this.searches.next({ date, process, productCode });
  }

  protected clear(): void {
    this.resetResults();
    this.filters.set({ date: '', process: '', productCode: '' });
  }

  private resetResults(): void {
    this.searches.next(null);
    this.report.set(null);
    this.error.set(null);
    this.invalidField.set(null);
    this.loading.set(false);
  }
}
