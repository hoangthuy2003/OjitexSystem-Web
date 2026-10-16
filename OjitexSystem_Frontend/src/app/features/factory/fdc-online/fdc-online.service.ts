import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { FdcOnlineReport } from './fdc-online.models';

@Injectable({ providedIn: 'root' })
export class FdcOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<FdcOnlineReport>('/api/factory/fdc-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
