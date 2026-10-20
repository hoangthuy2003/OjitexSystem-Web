import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { ConvertingHistoryFilters, ConvertingHistoryReport } from './converting-history.models';

@Injectable({ providedIn: 'root' })
export class ConvertingHistoryService {
  private readonly http = inject(HttpClient);

  getHistory(filters: ConvertingHistoryFilters) {
    const params: Record<string, string> = {};
    if (filters.date) params['date'] = filters.date;
    if (filters.process) params['process'] = filters.process;
    if (filters.productCode) params['productCode'] = filters.productCode;
    return this.http
      .get<ConvertingHistoryReport>('/api/factory/converting-history', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
