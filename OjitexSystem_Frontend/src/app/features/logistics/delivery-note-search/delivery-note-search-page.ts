import { ChangeDetectorRef, Component, DestroyRef, inject } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { apiErrorMessage } from '../../../core/http/api-errors';
import { DeliveryNote } from './delivery-note-search.models';
import { DeliveryNoteService } from './delivery-note.service';

@Component({
  imports: [FormsModule, DatePipe, DecimalPipe, RouterLink],
  selector: 'app-delivery-note-search-page',
  styleUrl: './delivery-note-search-page.scss',
  templateUrl: './delivery-note-search-page.html',
})
export class DeliveryNoteSearchPage {
  private readonly service = inject(DeliveryNoteService);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly destroyRef = inject(DestroyRef);
  private requestId = 0;

  deliverySlip = '';
  note: DeliveryNote | null = null;
  loading = false;
  errorMessage: string | null = null;
  pageIndex = 0;
  readonly pageSize = 15;

  get pageCount(): number {
    return Math.max(1, Math.ceil((this.note?.details.length ?? 0) / this.pageSize));
  }

  get pagedDetails(): DeliveryNote['details'] {
    const start = this.pageIndex * this.pageSize;
    return this.note?.details.slice(start, start + this.pageSize) ?? [];
  }

  goToPage(index: number): void {
    this.pageIndex = Math.min(Math.max(0, index), this.pageCount - 1);
  }

  search(): void {
    const slip = this.deliverySlip.trim();
    if (!slip) {
      this.requestId++;
      this.note = null;
      this.loading = false;
      this.errorMessage = 'Enter a delivery slip number.';
      return;
    }
    const requestId = ++this.requestId;
    this.loading = true;
    this.errorMessage = null;
    this.note = null;
    this.service
      .search(slip)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (note) => {
          if (requestId !== this.requestId) return;
          this.note = note;
          this.pageIndex = 0;
          this.loading = false;
          this.changeDetector.markForCheck();
        },
        error: (error: unknown) => {
          if (requestId !== this.requestId) return;
          this.loading = false;
          this.errorMessage =
            error instanceof HttpErrorResponse && error.status === 404
              ? 'No delivery note found for this slip number.'
              : apiErrorMessage(
                  error,
                  'Unable to search for this delivery note. Please try again.',
                );
          this.changeDetector.markForCheck();
        },
      });
  }

  clear(): void {
    this.requestId++;
    this.deliverySlip = '';
    this.note = null;
    this.pageIndex = 0;
    this.errorMessage = null;
    this.loading = false;
  }
}
