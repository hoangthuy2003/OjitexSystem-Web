import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, timeout } from 'rxjs';
import { API_REQUEST_TIMEOUT_MS } from '../../../core/http/api-policy';
import { Category } from '../../../core/auth/auth.models';
import { AdminUser, UserForm, UserRole } from './admin-user.models';

@Injectable({ providedIn: 'root' })
export class AdminDashboardService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/admin/dashboard';

  getUsers(): Observable<AdminUser[]> {
    return this.http
      .get<AdminUser[]>(this.endpoint)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getRoles(): Observable<UserRole[]> {
    return this.http
      .get<UserRole[]>(`${this.endpoint}/roles`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  getCategories(): Observable<Category[]> {
    return this.http
      .get<Category[]>(`${this.endpoint}/categories`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  createUser(user: UserForm): Observable<AdminUser> {
    return this.http
      .post<AdminUser>(this.endpoint, user)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  updateUser(user: UserForm): Observable<AdminUser> {
    return this.http
      .put<AdminUser>(`${this.endpoint}/${encodeURIComponent(user.userId)}`, {
        userFamilyName: user.userFamilyName,
        userFirstName: user.userFirstName,
        roles: user.roles,
        categoryIds: user.categoryIds,
      })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  resetPassword(userId: string): Observable<{ message: string }> {
    return this.http
      .post<{ message: string }>(
        `${this.endpoint}/${encodeURIComponent(userId)}/reset-password`,
        {},
      )
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  deleteUser(userId: string): Observable<void> {
    return this.http
      .delete<void>(`${this.endpoint}/${encodeURIComponent(userId)}`)
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }
}
