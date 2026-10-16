import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { EMPTY, Subject, catchError, switchMap } from 'rxjs';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  ModuleRegistry,
  PaginationModule,
  TooltipModule,
  themeQuartz,
} from 'ag-grid-community';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { EVOL_COLUMN_DEFS, EVOL_DEFAULT_COLUMN } from './evol-online-columns';
import { EvolOnlineReport } from './evol-online.models';
import { EvolOnlineService } from './evol-online.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

@Component({
  selector: 'app-evol-online-page',
  imports: [DatePipe, RouterLink, AgGridAngular],
  templateUrl: './evol-online-page.html',
  styleUrl: './evol-online-page.scss',
})
export class EvolOnlinePage {
  private readonly service = inject(EvolOnlineService);
  private readonly actions = new Subject<'refresh' | 'clear'>();
  protected readonly columns = EVOL_COLUMN_DEFS;
  protected readonly defaultColDef = EVOL_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<EvolOnlineReport | null>(null);
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
                          'Unable to load EVOL Online results. Please refresh.',
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
