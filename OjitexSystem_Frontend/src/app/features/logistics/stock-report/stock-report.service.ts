import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { StockReport, StockReportHistory, StockReportKind } from './stock-report.models';

@Injectable({ providedIn: 'root' })
export class StockReportService {
  private readonly http = inject(HttpClient);

  private endpoint(kind: StockReportKind): string {
    return `/api/StockReport/${kind}`;
  }

  searchByProductCode(kind: StockReportKind, productCode: string): Observable<StockReport> {
    return this.http
      .get<StockReport>(`${this.endpoint(kind)}/${encodeURIComponent(productCode)}`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getAll(kind: StockReportKind): Observable<StockReport> {
    return this.http
      .get<StockReport>(this.endpoint(kind))
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getHistory(
    kind: StockReportKind,
    productCode: number,
    reportDate: string | null,
  ): Observable<StockReportHistory> {
    return this.http
      .get<StockReportHistory>(`${this.endpoint(kind)}/${productCode}/history`, {
        params: kind === 'monthly' && reportDate ? { reportDate } : {},
      })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
