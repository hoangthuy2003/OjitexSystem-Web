import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import {
  DeliveryInformationFilters,
  DeliveryInformationReport,
  DeliveryNote,
} from './delivery-note-search.models';
import {
  DeliveryMonthlyFilters,
  DeliveryMonthlyReport,
} from '../delivery-note-monthly/delivery-note-monthly.models';

@Injectable({ providedIn: 'root' })
export class DeliveryNoteService {
  private readonly http = inject(HttpClient);

  search(deliverySlip: string): Observable<DeliveryNote> {
    return this.http
      .get<DeliveryNote>(`/api/DeliveryNote/${encodeURIComponent(deliverySlip)}`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getInformation(filters: DeliveryInformationFilters): Observable<DeliveryInformationReport> {
    let params = new HttpParams();
    if (filters.deliverySlip) params = params.set('deliverySlip', filters.deliverySlip);
    if (filters.customerCode) params = params.set('customerCode', filters.customerCode);
    if (filters.productCode) params = params.set('productCode', filters.productCode);
    if (filters.deliveryDate) params = params.set('deliveryDate', filters.deliveryDate);

    return this.http
      .get<DeliveryInformationReport>('/api/DeliveryNote/information', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getMonthly(filters: DeliveryMonthlyFilters): Observable<DeliveryMonthlyReport> {
    let params = new HttpParams();
    if (filters.deliverySlip) params = params.set('deliverySlip', filters.deliverySlip);
    if (filters.customerCode) params = params.set('customerCode', filters.customerCode);
    if (filters.productCode) params = params.set('productCode', filters.productCode);
    if (filters.fromDate) params = params.set('fromDate', filters.fromDate);
    if (filters.toDate) params = params.set('toDate', filters.toDate);

    return this.http
      .get<DeliveryMonthlyReport>('/api/DeliveryNote/monthly', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
