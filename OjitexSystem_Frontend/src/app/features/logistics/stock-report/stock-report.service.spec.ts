import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { TimeoutError } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { StockReportService } from './stock-report.service';

describe('StockReportService request lifecycle', () => {
  it('times out and cancels a stalled request instead of leaving the page loading', () => {
    vi.useFakeTimers();
    try {
      TestBed.configureTestingModule({
        providers: [provideHttpClient(), provideHttpClientTesting()],
      });
      const errors: unknown[] = [];
      TestBed.inject(StockReportService)
        .getAll('current')
        .subscribe({ error: (error) => errors.push(error) });
      const http = TestBed.inject(HttpTestingController);
      const request = http.expectOne('/api/StockReport/current');
      vi.advanceTimersByTime(API_REQUEST_TIMEOUT_MS);
      expect(errors[0]).toBeInstanceOf(TimeoutError);
      expect(request.cancelled).toBe(true);
      http.verify();
    } finally {
      vi.useRealTimers();
    }
  });
});
