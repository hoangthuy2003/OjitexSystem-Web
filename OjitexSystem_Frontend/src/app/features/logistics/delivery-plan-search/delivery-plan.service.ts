import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { DeliveryPlanFilters, DeliveryPlanSearchResult } from './delivery-plan-search.models';

@Injectable({ providedIn: 'root' })
export class DeliveryPlanService {
  private readonly http = inject(HttpClient);

  search(filters: DeliveryPlanFilters): Observable<DeliveryPlanSearchResult> {
    let params = new HttpParams();
    if (filters.deliveryDate) params = params.set('deliveryDate', filters.deliveryDate);
    if (filters.receiveNo) params = params.set('receiveNo', filters.receiveNo);

    return this.http
      .get<DeliveryPlanSearchResult>('/api/DeliveryPlan/search', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
