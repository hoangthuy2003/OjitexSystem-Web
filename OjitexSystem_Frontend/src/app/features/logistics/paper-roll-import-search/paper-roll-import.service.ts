import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import {
  PaperRollImportFilters,
  PaperRollImportSearchResult,
} from './paper-roll-import-search.models';

@Injectable({ providedIn: 'root' })
export class PaperRollImportService {
  private readonly http = inject(HttpClient);

  search(filters: PaperRollImportFilters): Observable<PaperRollImportSearchResult> {
    let params = new HttpParams();
    if (filters.paperLabel) params = params.set('paperLabel', filters.paperLabel);
    if (filters.gradeName) params = params.set('gradeName', filters.gradeName);
    if (filters.dateReceived) params = params.set('dateReceived', filters.dateReceived);

    return this.http
      .get<PaperRollImportSearchResult>('/api/PaperRollImport/search', { params })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
