import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../interfaces/auth.interface';
import { AdminUser, UserForm, UserRole } from '../interfaces/admin-dashboard.interface';

@Injectable({ providedIn: 'root' })
export class AdminDashboardService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/admin/dashboard';

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(this.endpoint);
  }

  getRoles(): Observable<UserRole[]> {
    return this.http.get<UserRole[]>(`${this.endpoint}/roles`);
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.endpoint}/categories`);
  }

  createUser(user: UserForm): Observable<AdminUser> {
    return this.http.post<AdminUser>(this.endpoint, user);
  }

  updateUser(user: UserForm): Observable<AdminUser> {
    return this.http.put<AdminUser>(`${this.endpoint}/${encodeURIComponent(user.userId)}`, {
      userFamilyName: user.userFamilyName,
      userFirstName: user.userFirstName,
      roles: user.roles,
      categoryIds: user.categoryIds,
    });
  }

  resetPassword(userId: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.endpoint}/${encodeURIComponent(userId)}/reset-password`,
      {},
    );
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${encodeURIComponent(userId)}`);
  }
}
