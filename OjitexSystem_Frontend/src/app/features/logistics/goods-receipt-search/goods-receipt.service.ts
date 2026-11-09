import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { GoodsReceiptFilters, GoodsReceiptSearchResult } from './goods-receipt-search.models';

@Injectable({ providedIn: 'root' })
export class GoodsReceiptService {
  private readonly http = inject(HttpClient);

  search(filters: GoodsReceiptFilters): Observable<GoodsReceiptSearchResult> {
    let params = new HttpParams();
    if (filters.dateReceipt) params = params.set('dateReceipt', filters.dateReceipt);
    if (filters.receiveNo) params = params.set('receiveNo', filters.receiveNo);

    return this.http
      .get<GoodsReceiptSearchResult>('/api/GoodsReceipt/search', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
