import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import {
  StockReport,
  StockReportHistory,
  StockReportKind,
} from '../interfaces/stock-report.interface';

@Injectable({ providedIn: 'root' })
export class StockReportService {
  private readonly http = inject(HttpClient);

  private endpoint(kind: StockReportKind): string {
    return kind === 'last' ? '/api/LastStock' : '/api/MonthlyStock';
  }

  searchByProductCode(kind: StockReportKind, productCode: string): Observable<StockReport> {
    return this.http.get<StockReport>(`${this.endpoint(kind)}/${encodeURIComponent(productCode)}`);
  }

  getAll(kind: StockReportKind): Observable<StockReport> {
    return this.http.get<StockReport>(this.endpoint(kind));
  }

  getHistory(
    kind: StockReportKind,
    productCode: number,
    reportDate: string | null,
  ): Observable<StockReportHistory> {
    return this.http.get<StockReportHistory>(`${this.endpoint(kind)}/${productCode}/history`, {
      params: kind === 'monthly' && reportDate ? { reportDate } : {},
    });
  }
}
