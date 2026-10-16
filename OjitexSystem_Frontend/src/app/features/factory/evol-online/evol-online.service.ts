import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { EvolOnlineReport } from './evol-online.models';

@Injectable({ providedIn: 'root' })
export class EvolOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<EvolOnlineReport>('/api/factory/evol-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
