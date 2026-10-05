import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentStock } from './current-stock.model';

@Injectable({ providedIn: 'root' })
export class CurrentStockApi {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/CurrentStock';

  getAll(): Observable<CurrentStock[]> {
    return this.http.get<CurrentStock[]>(this.endpoint);
  }

  getByProductCode(proCd: number): Observable<CurrentStock> {
    return this.http.get<CurrentStock>(`${this.endpoint}/${encodeURIComponent(proCd)}`);
  }
}
