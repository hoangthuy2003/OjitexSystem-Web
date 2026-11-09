import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EMPTY, Subject, catchError, switchMap } from 'rxjs';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  ModuleRegistry,
  PaginationModule,
  themeQuartz,
} from 'ag-grid-community';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { ReportClipboardDirective } from '../../../shared/report-clipboard/report-clipboard.directive';
import { CORRUGATOR_COLUMN_DEFS, CORRUGATOR_DEFAULT_COLUMN } from '../corrugator-report-columns';
import { CorrugatorReport } from '../corrugator-report.models';
import { isValidReportDate } from '../report-date';
import { CorrugatorHistoryService } from './corrugator-history.service';

ModuleRegistry.registerModules([CellStyleModule, ClientSideRowModelModule, PaginationModule]);

@Component({
  selector: 'app-corrugator-history-page',
  imports: [ReportClipboardDirective, DatePipe, FormsModule, RouterLink, AgGridAngular],
  templateUrl: './corrugator-history-page.html',
  styleUrl: './corrugator-history-page.scss',
})
export class CorrugatorHistoryPage {
  private readonly service = inject(CorrugatorHistoryService);
  private readonly searches = new Subject<string | null>();
  protected readonly columns = CORRUGATOR_COLUMN_DEFS;
  protected readonly defaultColDef = CORRUGATOR_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly searchDate = signal('');
  protected readonly report = signal<CorrugatorReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidDate = signal(false);

  constructor() {
    this.searches
      .pipe(
        switchMap((date) =>
          date === null
            ? EMPTY
            : this.service.getHistory(date).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Corrugator History. Please search again.',
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

  protected changeDate(value: string): void {
    this.searches.next(null);
    this.searchDate.set(value);
    this.report.set(null);
    this.error.set(null);
    this.invalidDate.set(false);
    this.loading.set(false);
  }

  protected search(): void {
    if (this.loading()) return;
    if (!isValidReportDate(this.searchDate())) {
      this.invalidDate.set(true);
      this.error.set('Enter a valid Corrugator date in mm/dd/yyyy format.');
      return;
    }
    this.report.set(null);
    this.error.set(null);
    this.loading.set(true);
    this.searches.next(this.searchDate());
  }

  protected clear(): void {
    this.changeDate('');
  }
}
