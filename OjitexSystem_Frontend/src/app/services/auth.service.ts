import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AuthenticatedUser, LoginResponse } from '../interfaces/auth.interface';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/auth';
  private readonly sessionKey = 'ojitex.auth';

  login(username: string, password: string, rememberMe: boolean): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.endpoint}/login`, { username, password })
      .pipe(tap((response) => this.saveSession(response, rememberMe)));
  }

  changePassword(currentPassword: string, newPassword: string): Observable<void> {
    return this.http.post<void>(`${this.endpoint}/change-password`, {
      currentPassword,
      newPassword,
    });
  }

  get accessToken(): string | null {
    return this.readSession()?.accessToken ?? null;
  }

  get currentUser(): AuthenticatedUser | null {
    return this.readSession()?.user ?? null;
  }

  get isAdmin(): boolean {
    return this.currentUser?.roles.includes('ADMIN') ?? false;
  }

  hasCategory(categoryId: string): boolean {
    return this.currentUser?.categories.some((category) => category.categoryId === categoryId) ?? false;
  }

  clearSession(): void {
    this.sessionStorage()?.removeItem(this.sessionKey);
    this.localStorage()?.removeItem(this.sessionKey);
  }

  private saveSession(response: LoginResponse, rememberMe: boolean): void {
    const storage = rememberMe ? this.localStorage() : this.sessionStorage();
    const otherStorage = rememberMe ? this.sessionStorage() : this.localStorage();
    otherStorage?.removeItem(this.sessionKey);
    storage?.setItem(this.sessionKey, JSON.stringify(response));
  }

  private readSession(): LoginResponse | null {
    const session = this.sessionStorage()?.getItem(this.sessionKey);
    const stored = session ?? this.localStorage()?.getItem(this.sessionKey);
    if (!stored) {
      return null;
    }

    try {
      return JSON.parse(stored) as LoginResponse;
    } catch {
      this.clearSession();
      return null;
    }
  }

  private sessionStorage(): Storage | null {
    return typeof sessionStorage === 'undefined' ? null : sessionStorage;
  }

  private localStorage(): Storage | null {
    return typeof localStorage === 'undefined' ? null : localStorage;
  }
}
