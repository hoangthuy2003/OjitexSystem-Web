import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { FgOnlineReport } from './fg-online.models';

@Injectable({ providedIn: 'root' })
export class FgOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<FgOnlineReport>('/api/factory/fg-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
