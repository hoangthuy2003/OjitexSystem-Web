import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { CorrugatorReport } from '../corrugator-report.models';

@Injectable({ providedIn: 'root' })
export class CorrugatorOnlineService {
  private readonly http = inject(HttpClient);

  getToday() {
    return this.http
      .get<CorrugatorReport>('/api/factory/corrugator-online')
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
