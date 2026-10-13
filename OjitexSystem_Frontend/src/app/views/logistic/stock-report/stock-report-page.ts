import {
  ChangeDetectorRef,
  Component,
  DestroyRef,
  ElementRef,
  ViewChild,
  inject,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe, DecimalPipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { TimeoutError } from 'rxjs';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellStyleModule,
  ClientSideRowModelModule,
  ColDef,
  CellDoubleClickedEvent,
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
} from '../../../interfaces/stock-report.interface';
import { StockReportService } from '../../../services/stock-report.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

@Component({
  imports: [FormsModule, AgGridAngular, DatePipe, DecimalPipe],
  selector: 'app-stock-report-page',
  styleUrl: '../current-stock/current-stock-page.scss',
  templateUrl: './stock-report-page.html',
})
export class StockReportPage {
  private readonly stockReportService = inject(StockReportService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  readonly reportKind: StockReportKind = inject(ActivatedRoute).snapshot.data['stockReport'];
  readonly reportTitle = this.reportKind === 'monthly' ? 'Monthly Stock' : 'Last Stock';
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
  readonly defaultColDef: ColDef<StockReportItem> = {
    flex: 1,
    minWidth: 120,
    resizable: true,
    sortable: true,
    headerClass: 'center-header',
    cellStyle: { textAlign: 'center' },
  };
  readonly columnDefs: ColDef<StockReportItem>[] = [
    ...(this.reportKind === 'monthly'
      ? [
          {
            field: 'reportDate',
            headerName: 'Date',
            valueFormatter: ({ value }) =>
              value ? new Date(value + 'T00:00:00').toLocaleDateString('en-US') : '—',
          } satisfies ColDef<StockReportItem>,
        ]
      : []),
    { field: 'productCode', headerName: 'Product Code' },
    {
      field: 'openingStock',
      headerName: 'Opening Stock',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'warehouseNg',
      headerName: 'NG Warehouse',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'disposal',
      headerName: 'Disposal',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'repair',
      headerName: 'Repair',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'production',
      headerName: 'Production',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'delivery',
      headerName: 'Delivery',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'closingStock',
      headerName: this.reportKind === 'monthly' ? 'Closing Stock' : 'Stock',
      cellClass: 'numeric-cell stock-history-cell',
      tooltipValueGetter: () => 'Double-click to view stock history',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
  ];

  get gridPagination(): boolean {
    return this.searchMode === 'all' || this.stockItems.length > 1;
  }

  onStockDoubleClicked(event: CellDoubleClickedEvent<StockReportItem>): void {
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
    const selection = window.getSelection();
    const clipboard = event.clipboardData;
    const gridElement = event.currentTarget;
    if (!selection || !clipboard || !(gridElement instanceof HTMLElement)) return;

    const startCell = this.getSelectedCell(selection.anchorNode);
    const endCell = this.getSelectedCell(selection.focusNode);
    if (!startCell || !endCell || startCell === endCell) return;

    const startRow = startCell.closest<HTMLElement>('.ag-row[row-index]');
    const endRow = endCell.closest<HTMLElement>('.ag-row[row-index]');
    const startRowIndex = Number(startRow?.getAttribute('row-index'));
    const endRowIndex = Number(endRow?.getAttribute('row-index'));
    const startColumnId = startCell.getAttribute('col-id');
    const endColumnId = endCell.getAttribute('col-id');
    const columnIds = this.columnDefs
      .map(({ field }) => field)
      .filter((field): field is keyof StockReportItem & string => field !== undefined);
    const startColumnIndex = columnIds.findIndex((columnId) => columnId === startColumnId);
    const endColumnIndex = columnIds.findIndex((columnId) => columnId === endColumnId);

    if (
      !Number.isInteger(startRowIndex) ||
      !Number.isInteger(endRowIndex) ||
      startColumnIndex < 0 ||
      endColumnIndex < 0
    ) {
      return;
    }

    const firstRow = Math.min(startRowIndex, endRowIndex);
    const lastRow = Math.max(startRowIndex, endRowIndex);
    const firstColumn = Math.min(startColumnIndex, endColumnIndex);
    const lastColumn = Math.max(startColumnIndex, endColumnIndex);
    const rows = Array.from(gridElement.querySelectorAll<HTMLElement>('.ag-row[row-index]'))
      .filter((row) => {
        const rowIndex = Number(row.getAttribute('row-index'));
        return rowIndex >= firstRow && rowIndex <= lastRow;
      })
      .sort(
        (left, right) =>
          Number(left.getAttribute('row-index')) - Number(right.getAttribute('row-index')),
      );

    const text = rows
      .map((row) => {
        const cells = new Map(
          Array.from(row.querySelectorAll<HTMLElement>('.ag-cell[col-id]')).map((cell) => [
            cell.getAttribute('col-id'),
            cell.textContent?.trim() ?? '',
          ]),
        );
        return columnIds
          .slice(firstColumn, lastColumn + 1)
          .map((columnId) => cells.get(columnId) ?? '')
          .join('\t');
      })
      .join('\n');

    if (!text) return;
    event.preventDefault();
    clipboard.setData('text/plain', text);
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

  private getSelectedCell(node: Node | null): HTMLElement | null {
    const element = node instanceof Element ? node : node?.parentElement;
    return element?.closest<HTMLElement>('.ag-cell[col-id]') ?? null;
  }
}
