import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { authInterceptor } from './services/auth.interceptor';

describe('App', () => {
  it('renders the route outlet', async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideRouter([])],
    }).compileComponents();

    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('router-outlet')).not.toBeNull();
  });

  it('allows a signed-in user to change a reset password', async () => {
    sessionStorage.setItem(
      'ojitex.auth',
      JSON.stringify({
        accessToken: 'test-token',
        expiresAt: '2026-10-10T08:00:00Z',
        user: {
          userId: 'trung',
          userFamilyName: null,
          userFirstName: null,
          roles: ['USER'],
          categories: [],
        },
      }),
    );

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(App);
    const http = TestBed.inject(HttpTestingController);
    fixture.detectChanges();

    fixture.nativeElement.querySelector('.change-password-button').click();
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="dialog"]')).not.toBeNull();

    const app = fixture.componentInstance;
    app.currentPassword = '123456';
    app.newPassword = 'new-password';
    app.confirmNewPassword = 'new-password';
    app.submitChangePassword();

    const request = http.expectOne('/api/auth/change-password');
    expect(request.request.method).toBe('POST');
    expect(request.request.headers.has('Authorization')).toBe(true);
    expect(request.request.body).toEqual({
      currentPassword: '123456',
      newPassword: 'new-password',
    });
    request.flush(null, { status: 204, statusText: 'No Content' });
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('[role="dialog"]')).toBeNull();
    expect(fixture.nativeElement.querySelector('[role="status"]').textContent).toContain(
      'Password changed successfully.',
    );
    http.verify();
    fixture.destroy();
    sessionStorage.removeItem('ojitex.auth');
  });

  it('does not submit when the new password confirmation differs', async () => {
    sessionStorage.setItem(
      'ojitex.auth',
      JSON.stringify({
        accessToken: 'test-token',
        expiresAt: '2026-10-10T08:00:00Z',
        user: {
          userId: 'trung',
          userFamilyName: null,
          userFirstName: null,
          roles: ['USER'],
          categories: [],
        },
      }),
    );

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(App);
    const http = TestBed.inject(HttpTestingController);
    const app = fixture.componentInstance;
    app.openChangePassword();
    app.currentPassword = '123456';
    app.newPassword = 'new-password';
    app.confirmNewPassword = 'different-password';
    app.submitChangePassword();

    expect(app.passwordChangeError).toContain('do not match');
    http.expectNone('/api/auth/change-password');
    fixture.destroy();
    sessionStorage.removeItem('ojitex.auth');
  });
});
