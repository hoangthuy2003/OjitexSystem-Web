import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, tap, timeout } from 'rxjs';
import { AuthenticatedUser, LoginResponse } from './auth.models';
import { AuthSessionStore } from './auth-session.store';
import { ADMIN_ROLE } from './access.constants';
import { API_REQUEST_TIMEOUT_MS } from '../http/api-policy';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly sessions = inject(AuthSessionStore);
  private readonly endpoint = '/api/auth';

  login(username: string, password: string, rememberMe: boolean): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.endpoint + '/login', { username, password }).pipe(
      timeout({ first: API_REQUEST_TIMEOUT_MS }),
      tap((response) => this.sessions.saveSession(response, rememberMe)),
    );
  }

  changePassword(currentPassword: string, newPassword: string): Observable<void> {
    return this.http
      .post<void>(this.endpoint + '/change-password', { currentPassword, newPassword })
      .pipe(timeout({ first: API_REQUEST_TIMEOUT_MS }));
  }

  get accessToken(): string | null {
    return this.sessions.readSession()?.accessToken ?? null;
  }
  get currentUser(): AuthenticatedUser | null {
    return this.sessions.readSession()?.user ?? null;
  }
  get isAdmin(): boolean {
    return this.currentUser?.roles.some((role) => role.toUpperCase() === ADMIN_ROLE) ?? false;
  }

  hasCategory(categoryId: string): boolean {
    return (
      this.currentUser?.categories.some((category) => category.categoryId === categoryId) ?? false
    );
  }

  clearSession(): void {
    this.sessions.clearSession();
  }
}
