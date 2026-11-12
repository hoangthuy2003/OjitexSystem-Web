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
  PAPER_ROLL_IMPORT_COLUMN_DEFS,
  PAPER_ROLL_IMPORT_DEFAULT_COLUMN,
} from './paper-roll-import-columns';
import {
  PaperRollImportFilters,
  PaperRollImportSearchResult,
} from './paper-roll-import-search.models';
import { PaperRollImportService } from './paper-roll-import.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

const EMPTY_FILTERS: PaperRollImportFilters = {
  paperLabel: '',
  gradeName: '',
  dateReceived: '',
};

@Component({
  selector: 'app-paper-roll-import-search-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './paper-roll-import-search-page.html',
  styleUrl: './paper-roll-import-search-page.scss',
})
export class PaperRollImportSearchPage {
  private readonly service = inject(PaperRollImportService);
  private readonly searches = new Subject<PaperRollImportFilters | null>();

  protected readonly filters = signal<PaperRollImportFilters>({ ...EMPTY_FILTERS });
  protected readonly columns = PAPER_ROLL_IMPORT_COLUMN_DEFS;
  protected readonly defaultColDef = PAPER_ROLL_IMPORT_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<PaperRollImportSearchResult | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof PaperRollImportFilters | null>(null);

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
                          'Unable to load Paper Roll Import Search. Please search again.',
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

  protected changeFilter(field: keyof PaperRollImportFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const paperLabel = current.paperLabel.trim();
    const gradeName = current.gradeName.trim();
    const dateReceived = current.dateReceived.trim();

    if (paperLabel.length > 10) {
      this.invalidField.set('paperLabel');
      this.error.set('Paper Label must contain at most 10 characters.');
      return;
    }
    if (gradeName.length > 15) {
      this.invalidField.set('gradeName');
      this.error.set('Grade Name must contain at most 15 characters.');
      return;
    }
    if (dateReceived && !isValidReportDate(dateReceived)) {
      this.invalidField.set('dateReceived');
      this.error.set('Enter a valid date in MM/DD/YYYY format.');
      return;
    }

    this.error.set(null);
    this.invalidField.set(null);
    this.report.set(null);
    this.loading.set(true);
    this.searches.next({ paperLabel, gradeName, dateReceived });
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
