import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { LoginResponse } from '../interfaces/auth.interface';
import { AuthService, AuthSessionError } from './auth.service';

describe('AuthService', () => {
  const key = 'ojitex.auth';
  let auth: AuthService;
  let http: HttpTestingController;

  function session(): LoginResponse {
    return {
      accessToken: 'test-token',
      expiresAt: new Date(Date.now() + 3600000).toISOString(),
      user: {
        userId: 'trung',
        userFamilyName: null,
        userFirstName: null,
        roles: ['ADMIN'],
        categories: [{ categoryId: 'logistics', categoryName: 'Logistics' }],
      },
    };
  }

  beforeEach(() => {
    localStorage.clear();
    sessionStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    auth = TestBed.inject(AuthService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
    vi.restoreAllMocks();
    localStorage.clear();
    sessionStorage.clear();
  });

  it('restores a valid session and its permissions', () => {
    localStorage.setItem(key, JSON.stringify(session()));
    expect(auth.accessToken).toBe('test-token');
    expect(auth.isAdmin).toBe(true);
    expect(auth.hasCategory('logistics')).toBe(true);
    expect(auth.hasCategory('other')).toBe(false);
  });

  it.each(['{broken', 'null', '[]', JSON.stringify({ accessToken: 'abc', user: {} })])(
    'rejects corrupt or malformed storage: %s',
    (stored) => {
      sessionStorage.setItem(key, stored);
      expect(auth.accessToken).toBeNull();
      expect(auth.currentUser).toBeNull();
      expect(auth.isAdmin).toBe(false);
      expect(auth.hasCategory('logistics')).toBe(false);
      expect(sessionStorage.getItem(key)).toBeNull();
    },
  );

  it.each(['expired', 'invalid-date', 'empty-token', 'bad-roles', 'bad-category'])(
    'rejects %s sessions',
    (kind) => {
      const value = session();
      if (kind === 'expired') value.expiresAt = new Date(Date.now() - 1).toISOString();
      if (kind === 'invalid-date') value.expiresAt = 'not-a-date';
      if (kind === 'empty-token') value.accessToken = '   ';
      if (kind === 'bad-roles') value.user.roles = null as unknown as string[];
      if (kind === 'bad-category') value.user.categories = [null as never];
      localStorage.setItem(key, JSON.stringify(value));
      expect(auth.accessToken).toBeNull();
      expect(localStorage.getItem(key)).toBeNull();
    },
  );

  it('expires a previously restored session', () => {
    sessionStorage.setItem(key, JSON.stringify(session()));
    expect(auth.accessToken).toBe('test-token');
    vi.spyOn(Date, 'now').mockReturnValue(Date.now() + 3600001);
    expect(auth.accessToken).toBeNull();
    expect(auth.currentUser).toBeNull();
    expect(sessionStorage.getItem(key)).toBeNull();
  });

  it('falls back to a valid local session if session storage is corrupt', () => {
    sessionStorage.setItem(key, '{}');
    localStorage.setItem(key, JSON.stringify(session()));
    expect(auth.accessToken).toBe('test-token');
    expect(sessionStorage.getItem(key)).toBeNull();
  });

  it('falls back when accessing session storage is blocked', () => {
    localStorage.setItem(key, JSON.stringify(session()));
    vi.spyOn(window, 'sessionStorage', 'get').mockImplementation(() => {
      throw new DOMException('Blocked', 'SecurityError');
    });
    expect(auth.accessToken).toBe('test-token');
  });

  it('handles getItem failures without throwing', () => {
    vi.spyOn(Storage.prototype, 'getItem').mockImplementation(() => {
      throw new DOMException('Blocked', 'SecurityError');
    });
    expect(auth.accessToken).toBeNull();
  });

  it.each([true, false])('saves login in the correct storage with rememberMe=%s', (rememberMe) => {
    const destination = rememberMe ? localStorage : sessionStorage;
    const other = rememberMe ? sessionStorage : localStorage;
    other.setItem(key, JSON.stringify(session()));
    const next = vi.fn();
    auth.login('trung', 'password', rememberMe).subscribe(next);
    const request = http.expectOne('/api/auth/login');
    expect(request.request.body).toEqual({ username: 'trung', password: 'password' });
    request.flush(session());
    expect(next).toHaveBeenCalled();
    expect(destination.getItem(key)).toContain('test-token');
    expect(other.getItem(key)).toBeNull();
  });

  it('preserves the previous session if saving fails', () => {
    const previous = { ...session(), accessToken: 'previous-token' };
    sessionStorage.setItem(key, JSON.stringify(previous));
    vi.spyOn(Storage.prototype, 'setItem').mockImplementation(() => {
      throw new DOMException('Full', 'QuotaExceededError');
    });
    const error = vi.fn();
    auth.login('trung', 'password', true).subscribe({ error });
    http.expectOne('/api/auth/login').flush(session());
    expect(error.mock.calls[0][0]).toBeInstanceOf(AuthSessionError);
    expect(sessionStorage.getItem(key)).toContain('previous-token');
    expect(auth.accessToken).toBe('previous-token');
  });

  it('rejects invalid login responses before saving them', () => {
    const error = vi.fn();
    auth.login('trung', 'password', false).subscribe({ error });
    http.expectOne('/api/auth/login').flush({ accessToken: 'abc', user: {} });
    expect(error.mock.calls[0][0]).toBeInstanceOf(AuthSessionError);
    expect(sessionStorage.getItem(key)).toBeNull();
  });

  it('logs out in memory even if removing stored data fails', () => {
    sessionStorage.setItem(key, JSON.stringify(session()));
    expect(auth.accessToken).toBe('test-token');
    vi.spyOn(Storage.prototype, 'removeItem').mockImplementation(() => {
      throw new DOMException('Blocked', 'SecurityError');
    });
    expect(() => auth.clearSession()).not.toThrow();
    expect(auth.accessToken).toBeNull();
    expect(auth.currentUser).toBeNull();
  });

  it('updates cached state after a storage change from another tab', () => {
    localStorage.setItem(key, JSON.stringify(session()));
    expect(auth.accessToken).toBe('test-token');
    localStorage.removeItem(key);
    window.dispatchEvent(new StorageEvent('storage', { key }));
    expect(auth.accessToken).toBeNull();
  });
});
