import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { CurrentStock } from '../interfaces/current-stock.interface';

@Injectable({ providedIn: 'root' })
export class CurrentStockService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/CurrentStock';
  private readonly requestTimeoutMs = 15_000;

  searchByProductCodePrefix(productCodePrefix: string): Observable<CurrentStock[]> {
    return this.http.get<CurrentStock[]>(
      `${this.endpoint}/search/${encodeURIComponent(productCodePrefix)}`,
    ).pipe(timeout({ first: this.requestTimeoutMs }));
  }

  getAll(): Observable<CurrentStock[]> {
    return this.http
      .get<CurrentStock[]>(this.endpoint)
      .pipe(timeout({ first: this.requestTimeoutMs }));
  }
}
