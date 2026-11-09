import { ChangeDetectorRef, Component, DestroyRef, inject } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { GoodsReceiptItem } from './goods-receipt-search.models';
import { GoodsReceiptService } from './goods-receipt.service';

@Component({
  imports: [FormsModule, DatePipe, DecimalPipe, RouterLink],
  selector: 'app-goods-receipt-search-page',
  styleUrl: './goods-receipt-search-page.scss',
  templateUrl: './goods-receipt-search-page.html',
})
export class GoodsReceiptSearchPage {
  private readonly service = inject(GoodsReceiptService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);
  private requestId = 0;

  dateReceipt = '';
  receiveNo = '';
  items: GoodsReceiptItem[] = [];
  loading = false;
  errorMessage: string | null = null;
  hasSearched = false;
  pageIndex = 0;
  readonly pageSize = 20;

  get pageCount(): number {
    return Math.max(1, Math.ceil(this.items.length / this.pageSize));
  }

  get pagedItems(): GoodsReceiptItem[] {
    const start = this.pageIndex * this.pageSize;
    return this.items.slice(start, start + this.pageSize);
  }

  /** Trim then fall back to dash for blank/null strings. */
  text(value: string | null | undefined): string {
    return value?.trim() || '—';
  }

  goToPage(index: number): void {
    this.pageIndex = Math.min(Math.max(0, index), this.pageCount - 1);
  }

  search(): void {
    const requestId = ++this.requestId;
    this.loading = true;
    this.errorMessage = null;
    this.items = [];
    this.hasSearched = false;

    this.service
      .search({ dateReceipt: this.dateReceipt.trim(), receiveNo: this.receiveNo.trim() })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result) => {
          if (requestId !== this.requestId) return;
          this.items = result.items;
          this.pageIndex = 0;
          this.loading = false;
          this.hasSearched = true;
          this.changeDetector.markForCheck();
        },
        error: (error: unknown) => {
          if (requestId !== this.requestId) return;
          this.loading = false;
          this.errorMessage = apiErrorMessage(
            error,
            'Unable to search for goods receipts. Please try again.',
          );
          this.changeDetector.markForCheck();
        },
      });
  }

  clear(): void {
    this.requestId++;
    this.dateReceipt = '';
    this.receiveNo = '';
    this.items = [];
    this.pageIndex = 0;
    this.errorMessage = null;
    this.loading = false;
    this.hasSearched = false;
  }
}
