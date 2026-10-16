import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  ElementRef,
  ViewChild,
  inject,
} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe, DecimalPipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { TimeoutError } from 'rxjs';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  CellClickedEvent,
  ModuleRegistry,
  PaginationModule,
  TooltipModule,
  themeQuartz,
} from 'ag-grid-community';
import {
  StockReport,
  StockReportItem,
  StockReportHistory,
  StockReportKind,
} from './stock-report.models';
import { STOCK_DEFAULT_COLUMN, STOCK_REPORT_TITLES, stockReportColumns } from './stock-report-grid';
import { copyStockGridRange } from './stock-grid-clipboard';
import { StockReportService } from './stock-report.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

@Component({
  imports: [FormsModule, AgGridAngular, DatePipe, DecimalPipe, RouterLink],
  selector: 'app-stock-report-page',
  styleUrl: './stock-report-page.scss',
  templateUrl: './stock-report-page.html',
})
export class StockReportPage {
  private readonly stockReportService = inject(StockReportService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  readonly reportKind: StockReportKind = inject(ActivatedRoute).snapshot.data['stockReport'];
  readonly reportTitle = STOCK_REPORT_TITLES[this.reportKind];
  reportPeriod: string | null = null;
  historyReportDate: string | null = null;
  private requestId = 0;
  private readonly destroyRef = inject(DestroyRef);
  private historyRequestId = 0;
  @ViewChild('historyDialog') historyDialog!: ElementRef<HTMLDialogElement>;
  history: StockReportHistory | null = null;
  historyProductCode: number | null = null;
  historyLoading = false;
  historyError: string | null = null;

  stockItems: StockReportItem[] = [];
  private allStockReport: StockReport | null = null;
  searchMode: 'idle' | 'product' | 'all' = 'idle';
  loading = false;
  errorMessage: string | null = null;
  searchTerm = '';
  readonly gridTheme = themeQuartz;
  readonly defaultColDef = STOCK_DEFAULT_COLUMN;
  readonly columnDefs = stockReportColumns(this.reportKind);

  get gridPagination(): boolean {
    return this.searchMode === 'all' || this.stockItems.length > 1;
  }

  onStockClicked(event: CellClickedEvent<StockReportItem>): void {
    if (event.colDef.field !== 'closingStock' || event.data?.productCode == null) return;
    this.openHistory(event.data.productCode, event.data.reportDate);
  }

  openHistory(productCode: number, reportDate: string | null = this.historyReportDate): void {
    this.historyReportDate = reportDate;
    this.historyProductCode = productCode;
    this.history = null;
    this.historyError = null;
    this.historyLoading = true;
    if (!this.historyDialog.nativeElement.open) this.historyDialog.nativeElement.showModal();
    const requestId = ++this.historyRequestId;
    if (this.reportKind === 'monthly' && !reportDate) {
      this.historyLoading = false;
      this.historyError =
        'This stock record has no report date. Its history month cannot be selected.';
      return;
    }
    this.stockReportService
      .getHistory(this.reportKind, productCode, reportDate)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (history) => {
          if (requestId !== this.historyRequestId) return;
          this.history = history;
          this.historyLoading = false;
          this.changeDetector.markForCheck();
        },
        error: () => {
          if (requestId !== this.historyRequestId) return;
          this.historyLoading = false;
          this.historyError = 'Unable to load stock history. Please try again.';
          this.changeDetector.markForCheck();
        },
      });
  }

  closeHistory(): void {
    this.historyRequestId++;
    this.historyDialog.nativeElement.close();
  }

  printHistory(): void {
    if (this.history && !this.historyLoading && !this.historyError) window.print();
  }

  copyGridRange(event: ClipboardEvent): void {
    copyStockGridRange(event, this.columnDefs);
  }

  searchByProductCode(): void {
    const query = this.searchTerm.trim();
    if (!/^\d{1,7}$/.test(query)) {
      this.requestId++;
      this.searchMode = 'product';
      this.loading = false;
      this.stockItems = [];
      this.errorMessage = 'Enter a product code containing no more than 7 digits.';
      return;
    }

    const requestId = ++this.requestId;
    this.searchMode = 'product';
    this.loading = true;
    this.errorMessage = null;
    this.stockItems = [];

    this.stockReportService
      .searchByProductCode(this.reportKind, query)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (report) => {
          if (requestId !== this.requestId) return;
          this.stockItems = report.items;
          this.reportPeriod = report.workingDayFrom;
          this.loading = false;
          this.changeDetector.markForCheck();
        },
        error: (error: unknown) => {
          if (requestId !== this.requestId) return;
          this.loading = false;
          if (error instanceof TimeoutError) {
            this.errorMessage =
              'The API request timed out (15 seconds). Check your connection and try again.';
          } else {
            this.errorMessage =
              'Unable to search for this product code. Check the API connection and try again.';
          }
          this.changeDetector.markForCheck();
        },
      });
  }

  searchAll(): void {
    this.searchTerm = '';
    this.searchMode = 'all';
    this.loadAll();
  }

  refresh(): void {
    if (this.searchMode === 'all') {
      this.loadAll(true);
    } else if (this.searchMode === 'product') {
      this.searchByProductCode();
    }
  }

  private loadAll(forceRefresh = false): void {
    if (this.allStockReport && !forceRefresh) {
      this.requestId++;
      this.stockItems = this.allStockReport.items;
      this.reportPeriod = this.allStockReport.workingDayFrom;
      this.loading = false;
      this.errorMessage = null;
      return;
    }

    const requestId = ++this.requestId;
    this.loading = true;
    this.errorMessage = null;
    this.stockItems = [];

    this.stockReportService
      .getAll(this.reportKind)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (report) => {
          if (requestId !== this.requestId) return;
          this.allStockReport = report;
          this.stockItems = report.items;
          this.reportPeriod = report.workingDayFrom;
          this.loading = false;
          this.changeDetector.markForCheck();
        },
        error: (error: unknown) => {
          if (requestId !== this.requestId) return;
          this.loading = false;
          this.errorMessage =
            error instanceof TimeoutError
              ? 'The API request timed out (15 seconds). Check your connection and try again.'
              : 'Unable to load the stock list. Please try again.';
          this.changeDetector.markForCheck();
        },
      });
  }
}
