import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { FpsOnlineReport } from './fps-online.models';

@Injectable({ providedIn: 'root' })
export class FpsOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<FpsOnlineReport>('/api/factory/fps-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
