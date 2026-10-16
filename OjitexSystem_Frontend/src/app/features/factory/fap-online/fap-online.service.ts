import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { FapOnlineReport } from './fap-online.models';

@Injectable({ providedIn: 'root' })
export class FapOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<FapOnlineReport>('/api/factory/fap-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
