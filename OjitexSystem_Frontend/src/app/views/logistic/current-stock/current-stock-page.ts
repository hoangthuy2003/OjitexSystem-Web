import { ChangeDetectorRef, Component, DestroyRef, ElementRef, ViewChild, inject } from '@angular/core';
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
import { CurrentStock, CurrentStockHistory } from '../../../interfaces/current-stock.interface';
import { CurrentStockService } from '../../../services/current-stock.service';

ModuleRegistry.registerModules([CellStyleModule, ClientSideRowModelModule, PaginationModule, TooltipModule]);

@Component({
  imports: [FormsModule, AgGridAngular, DatePipe, DecimalPipe],
  selector: 'app-current-stock-page',
  styleUrl: './current-stock-page.scss',
  templateUrl: './current-stock-page.html',
})
export class CurrentStockPage {
  private readonly currentStockService = inject(CurrentStockService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private requestId = 0;
  private readonly destroyRef = inject(DestroyRef);
  private historyRequestId = 0;
  @ViewChild('historyDialog') historyDialog!: ElementRef<HTMLDialogElement>;
  history: CurrentStockHistory | null = null;
  historyProductCode: number | null = null;
  historyLoading = false;
  historyError: string | null = null;

  stockItems: CurrentStock[] = [];
  private allStockItems: CurrentStock[] | null = null;
  searchMode: 'idle' | 'product' | 'all' = 'idle';
  loading = false;
  errorMessage: string | null = null;
  searchTerm = '';
  readonly gridTheme = themeQuartz;
  readonly defaultColDef: ColDef<CurrentStock> = {
    flex: 1,
    minWidth: 120,
    resizable: true,
    sortable: true,
    headerClass: 'center-header',
    cellStyle: { textAlign: 'center' },
  };
  readonly columnDefs: ColDef<CurrentStock>[] = [
    { field: 'cstProCd', headerName: 'Product Code' },
    {
      field: 'cstOpenStock',
      headerName: 'Opening Stock',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstWarehouseNg',
      headerName: 'NG Warehouse',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstDisposal',
      headerName: 'Disposal',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstRepair',
      headerName: 'Repair',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstProduction',
      headerName: 'Production',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstDelivery',
      headerName: 'Delivery',
      cellClass: 'numeric-cell',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
    {
      field: 'cstStock',
      headerName: 'Current Stock',
      cellClass: 'numeric-cell stock-history-cell',
      tooltipValueGetter: () => 'Double-click to view stock history',
      valueFormatter: ({ value }) => (value == null ? '—' : value.toLocaleString()),
    },
  ];

  get gridPagination(): boolean {
    return this.searchMode === 'all' || this.stockItems.length > 1;
  }

  onStockDoubleClicked(event: CellDoubleClickedEvent<CurrentStock>): void {
    if (event.colDef.field !== 'cstStock' || !event.data) return;
    this.openHistory(event.data.cstProCd);
  }

  openHistory(productCode: number): void {
    this.historyProductCode = productCode;
    this.history = null;
    this.historyError = null;
    this.historyLoading = true;
    if (!this.historyDialog.nativeElement.open) this.historyDialog.nativeElement.showModal();
    const requestId = ++this.historyRequestId;
    this.currentStockService.getHistory(productCode).pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
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
      .filter((field): field is keyof CurrentStock & string => field !== undefined);
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

    this.currentStockService.searchByProductCode(query).subscribe({
      next: (stocks) => {
        if (requestId !== this.requestId) return;
        this.stockItems = stocks;
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
    if (this.allStockItems && !forceRefresh) {
      this.stockItems = this.allStockItems;
      return;
    }

    const requestId = ++this.requestId;
    this.loading = true;
    this.errorMessage = null;
    this.stockItems = [];

    this.currentStockService.getAll().subscribe({
      next: (items) => {
        if (requestId !== this.requestId) return;
        this.allStockItems = items;
        this.stockItems = items;
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
