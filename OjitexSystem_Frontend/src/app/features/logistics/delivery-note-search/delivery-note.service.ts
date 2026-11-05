import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { DeliveryNote } from './delivery-note-search.models';

@Injectable({ providedIn: 'root' })
export class DeliveryNoteService {
  private readonly http = inject(HttpClient);

  search(deliverySlip: string): Observable<DeliveryNote> {
    return this.http
      .get<DeliveryNote>(`/api/DeliveryNote/${encodeURIComponent(deliverySlip)}`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
