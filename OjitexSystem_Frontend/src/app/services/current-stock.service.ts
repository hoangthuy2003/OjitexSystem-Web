import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrentStock, CurrentStockHistory } from '../interfaces/current-stock.interface';

@Injectable({ providedIn: 'root' })
export class CurrentStockService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/CurrentStock';

  searchByProductCode(productCode: string): Observable<CurrentStock[]> {
    return this.http.get<CurrentStock[]>(`${this.endpoint}/${encodeURIComponent(productCode)}`);
  }

  getAll(): Observable<CurrentStock[]> {
    return this.http.get<CurrentStock[]>(this.endpoint);
  }

  getHistory(productCode: number): Observable<CurrentStockHistory> {
    return this.http.get<CurrentStockHistory>(`${this.endpoint}/${productCode}/history`);
  }
}
