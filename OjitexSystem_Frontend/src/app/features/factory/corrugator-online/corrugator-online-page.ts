import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { EMPTY, Subject, catchError, switchMap } from 'rxjs';
import { AgGridAngular } from 'ag-grid-angular';
import { ReportClipboardDirective } from '../../../shared/report-clipboard/report-clipboard.directive';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  ModuleRegistry,
  PaginationModule,
  themeQuartz,
} from 'ag-grid-community';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { CORRUGATOR_COLUMN_DEFS, CORRUGATOR_DEFAULT_COLUMN } from '../corrugator-report-columns';
import { CorrugatorReport } from '../corrugator-report.models';
import { CorrugatorOnlineService } from './corrugator-online.service';

ModuleRegistry.registerModules([CellStyleModule, ClientSideRowModelModule, PaginationModule]);

@Component({
  selector: 'app-corrugator-online-page',
  imports: [ReportClipboardDirective, DatePipe, RouterLink, AgGridAngular],
  templateUrl: './corrugator-online-page.html',
  styleUrl: './corrugator-online-page.scss',
})
export class CorrugatorOnlinePage {
  private readonly service = inject(CorrugatorOnlineService);
  private readonly actions = new Subject<'refresh' | 'clear'>();
  protected readonly columns = CORRUGATOR_COLUMN_DEFS;
  protected readonly defaultColDef = CORRUGATOR_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<CorrugatorReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly cleared = signal(false);

  constructor() {
    this.actions
      .pipe(
        switchMap((action) =>
          action === 'clear'
            ? EMPTY
            : this.service.getToday().pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Corrugator Online results. Please refresh.',
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
    this.refresh();
  }

  protected refresh(): void {
    this.report.set(null);
    this.error.set(null);
    this.cleared.set(false);
    this.loading.set(true);
    this.actions.next('refresh');
  }

  protected clear(): void {
    this.actions.next('clear');
    this.report.set(null);
    this.error.set(null);
    this.loading.set(false);
    this.cleared.set(true);
  }
}
