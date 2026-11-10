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
  GOODS_RECEIPT_INFORMATION_COLUMN_DEFS,
  GOODS_RECEIPT_INFORMATION_DEFAULT_COLUMN,
} from './goods-receipt-information-columns';
import {
  GoodsReceiptInformationFilters,
  GoodsReceiptInformationReport,
} from './goods-receipt-information.models';
import { GoodsReceiptService } from '../goods-receipt-search/goods-receipt.service';

ModuleRegistry.registerModules([
  CellStyleModule,
  ClientSideRowModelModule,
  PaginationModule,
  TooltipModule,
]);

const EMPTY_FILTERS: GoodsReceiptInformationFilters = {
  dateReceipt: '',
  receiveNo: '',
  productCode: '',
};

@Component({
  selector: 'app-goods-receipt-information-page',
  imports: [FormsModule, RouterLink, AgGridAngular, ReportClipboardDirective],
  templateUrl: './goods-receipt-information-page.html',
  styleUrl: './goods-receipt-information-page.scss',
})
export class GoodsReceiptInformationPage {
  private readonly service = inject(GoodsReceiptService);
  private readonly searches = new Subject<GoodsReceiptInformationFilters | null>();

  protected readonly filters = signal<GoodsReceiptInformationFilters>({ ...EMPTY_FILTERS });
  protected readonly columns = GOODS_RECEIPT_INFORMATION_COLUMN_DEFS;
  protected readonly defaultColDef = GOODS_RECEIPT_INFORMATION_DEFAULT_COLUMN;
  protected readonly gridTheme = themeQuartz.withParams({
    wrapperBorderRadius: 0,
    fontSize: 13,
    cellHorizontalPadding: 8,
    rowHeight: 32,
    headerHeight: 44,
  });
  protected readonly report = signal<GoodsReceiptInformationReport | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly invalidField = signal<keyof GoodsReceiptInformationFilters | null>(null);

  constructor() {
    this.searches
      .pipe(
        switchMap((filters) =>
          filters === null
            ? EMPTY
            : this.service.getInformation(filters).pipe(
                catchError((error: unknown) => {
                  this.loading.set(false);
                  this.error.set(
                    error instanceof HttpErrorResponse && error.status === 403
                      ? 'Your account does not have access to this report.'
                      : apiErrorMessage(
                          error,
                          'Unable to load Goods Receipt Information. Please search again.',
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

  protected changeFilter(field: keyof GoodsReceiptInformationFilters, value: string): void {
    this.resetResults();
    this.filters.update((filters) => ({ ...filters, [field]: value }));
  }

  protected search(): void {
    if (this.loading()) return;
    const current = this.filters();
    const dateReceipt = current.dateReceipt.trim();
    const receiveNo = current.receiveNo.trim();
    const productCode = current.productCode.trim();

    if (dateReceipt && !isValidReportDate(dateReceipt)) {
      this.invalidField.set('dateReceipt');
      this.error.set('Enter a valid date in mm/dd/yyyy format.');
      return;
    }
    if (receiveNo.length > 20) {
      this.invalidField.set('receiveNo');
      this.error.set('Receive no must contain at most 20 characters.');
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
    this.searches.next({ dateReceipt, receiveNo, productCode });
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
