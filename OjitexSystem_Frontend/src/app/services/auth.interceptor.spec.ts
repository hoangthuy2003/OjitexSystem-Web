import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Router, provideRouter } from '@angular/router';
import { authInterceptor } from './auth.interceptor';
import { AuthService } from './auth.service';

describe('authInterceptor', () => {
  let client: HttpClient;
  let http: HttpTestingController;
  let auth: AuthService;
  let navigate: ReturnType<typeof vi.spyOn>;

  beforeEach(() => {
    localStorage.clear();
    sessionStorage.clear();
    sessionStorage.setItem(
      'ojitex.auth',
      JSON.stringify({
        accessToken: 'test-token',
        expiresAt: new Date(Date.now() + 3600000).toISOString(),
        user: {
          userId: 'trung',
          userFamilyName: null,
          userFirstName: null,
          roles: [],
          categories: [],
        },
      }),
    );
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    });
    client = TestBed.inject(HttpClient);
    http = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
    navigate = vi.spyOn(TestBed.inject(Router), 'navigateByUrl').mockResolvedValue(true);
  });

  afterEach(() => {
    http.verify();
    vi.restoreAllMocks();
    sessionStorage.clear();
    localStorage.clear();
  });

  it('clears the rejected session, redirects, and propagates a 401', () => {
    const error = vi.fn();
    client.get('/api/auth/me').subscribe({ error });
    const request = http.expectOne('/api/auth/me');
    expect(request.request.headers.get('Authorization')).toBe('Bearer test-token');
    request.flush(null, { status: 401, statusText: 'Unauthorized' });
    expect(auth.accessToken).toBeNull();
    expect(navigate).toHaveBeenCalledWith('/login', { replaceUrl: true });
    expect(error.mock.calls[0][0].status).toBe(401);
  });

  it.each(['/api/auth/login', '/api/auth/login?source=test', 'https://example.com/data'])(
    'does not attach tokens or log out for %s',
    (url) => {
      client.post(url, {}).subscribe({ error: () => {} });
      const request = http.expectOne(url);
      expect(request.request.headers.has('Authorization')).toBe(false);
      request.flush(null, { status: 401, statusText: 'Unauthorized' });
      expect(auth.accessToken).toBe('test-token');
      expect(navigate).not.toHaveBeenCalled();
    },
  );

  it('keeps the session on 403', () => {
    client.get('/api/admin').subscribe({ error: () => {} });
    http.expectOne('/api/admin').flush(null, { status: 403, statusText: 'Forbidden' });
    expect(auth.accessToken).toBe('test-token');
    expect(navigate).not.toHaveBeenCalled();
  });

  it('redirects when the token expires while a request is in flight', () => {
    client.get('/api/auth/me').subscribe({ error: () => {} });
    const request = http.expectOne('/api/auth/me');
    vi.spyOn(Date, 'now').mockReturnValue(Date.now() + 3600001);
    request.flush(null, { status: 401, statusText: 'Unauthorized' });
    expect(auth.accessToken).toBeNull();
    expect(navigate).toHaveBeenCalledWith('/login', { replaceUrl: true });
  });

  it('redirects on 401 when the session expired before the request', () => {
    vi.spyOn(Date, 'now').mockReturnValue(Date.now() + 3600001);
    client.get('/api/auth/me').subscribe({ error: () => {} });
    const request = http.expectOne('/api/auth/me');
    expect(request.request.headers.has('Authorization')).toBe(false);
    request.flush(null, { status: 401, statusText: 'Unauthorized' });
    expect(navigate).toHaveBeenCalledWith('/login', { replaceUrl: true });
  });

  it('does not clear a newer login when an older request returns 401', () => {
    client.get('/api/auth/me').subscribe({ error: () => {} });
    const oldRequest = http.expectOne('/api/auth/me');
    auth.login('trung', 'password', false).subscribe();
    http.expectOne('/api/auth/login').flush({
      accessToken: 'new-token',
      expiresAt: new Date(Date.now() + 3600000).toISOString(),
      user: {
        userId: 'trung',
        userFamilyName: null,
        userFirstName: null,
        roles: [],
        categories: [],
      },
    });
    oldRequest.flush(null, { status: 401, statusText: 'Unauthorized' });
    expect(auth.accessToken).toBe('new-token');
    expect(navigate).not.toHaveBeenCalled();
  });
});
