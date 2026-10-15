import { DestroyRef, Injectable, inject } from '@angular/core';
import { LoginResponse } from './auth.models';

export class AuthSessionError extends Error {}

@Injectable({ providedIn: 'root' })
export class AuthSessionStore {
  private readonly sessionKey = 'ojitex.auth';
  private session: LoginResponse | null | undefined;

  constructor() {
    if (typeof window !== 'undefined') {
      const onStorage = (event: StorageEvent) => {
        if (event.key === this.sessionKey || event.key === null) {
          this.session = undefined;
        }
      };
      window.addEventListener('storage', onStorage);
      inject(DestroyRef).onDestroy(() => window.removeEventListener('storage', onStorage));
    }
  }

  clearSession(): void {
    this.session = null;
    this.removeStoredSession('sessionStorage');
    this.removeStoredSession('localStorage');
  }

  saveSession(response: LoginResponse, rememberMe: boolean): void {
    if (!this.isValidSession(response)) {
      throw new AuthSessionError(
        'The server returned an invalid or expired sign-in session. Please try again.',
      );
    }

    const storage = this.getStorage(rememberMe ? 'localStorage' : 'sessionStorage');
    try {
      if (!storage) {
        throw new Error('Storage unavailable');
      }
      storage.setItem(this.sessionKey, JSON.stringify(response));
    } catch {
      throw new AuthSessionError(
        'Unable to save your sign-in session. Allow browser storage and try again.',
      );
    }

    this.session = response;
    this.removeStoredSession(rememberMe ? 'sessionStorage' : 'localStorage');
  }

  readSession(): LoginResponse | null {
    if (this.session === undefined) {
      this.session =
        this.readStoredSession('sessionStorage') ?? this.readStoredSession('localStorage');
    }
    if (this.session && !this.isValidSession(this.session)) {
      this.clearSession();
    }
    return this.session;
  }

  private isValidSession(value: unknown): value is LoginResponse {
    if (!this.isRecord(value)) {
      return false;
    }
    const user = value['user'];
    return (
      typeof value['accessToken'] === 'string' &&
      value['accessToken'].trim().length > 0 &&
      typeof value['expiresAt'] === 'string' &&
      Date.parse(value['expiresAt']) > Date.now() &&
      this.isRecord(user) &&
      typeof user['userId'] === 'string' &&
      user['userId'].trim().length > 0 &&
      (user['userFamilyName'] === null || typeof user['userFamilyName'] === 'string') &&
      (user['userFirstName'] === null || typeof user['userFirstName'] === 'string') &&
      Array.isArray(user['roles']) &&
      user['roles'].every((role) => typeof role === 'string') &&
      Array.isArray(user['categories']) &&
      user['categories'].every(
        (category) =>
          this.isRecord(category) &&
          typeof category['categoryId'] === 'string' &&
          typeof category['categoryName'] === 'string',
      )
    );
  }

  private isRecord(value: unknown): value is Record<string, unknown> {
    return typeof value === 'object' && value !== null && !Array.isArray(value);
  }

  private getStorage(name: 'sessionStorage' | 'localStorage'): Storage | null {
    try {
      return typeof window === 'undefined' ? null : window[name];
    } catch {
      return null;
    }
  }

  private readStoredSession(name: 'sessionStorage' | 'localStorage'): LoginResponse | null {
    try {
      const stored = this.getStorage(name)?.getItem(this.sessionKey);
      if (!stored) {
        return null;
      }
      const parsed: unknown = JSON.parse(stored);
      if (this.isValidSession(parsed)) {
        return parsed;
      }
    } catch {
      // A corrupt or inaccessible store must not prevent checking the other store.
    }
    this.removeStoredSession(name);
    return null;
  }

  private removeStoredSession(name: 'sessionStorage' | 'localStorage'): void {
    try {
      this.getStorage(name)?.removeItem(this.sessionKey);
    } catch {
      // The in-memory session remains authoritative when storage is unavailable.
    }
  }
}
