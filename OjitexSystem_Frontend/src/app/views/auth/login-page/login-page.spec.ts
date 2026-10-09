import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router, provideRouter } from '@angular/router';
import { TestBed } from '@angular/core/testing';
import { LoginPage } from './login-page';

describe('LoginPage', () => {
  async function createPage() {
    await TestBed.configureTestingModule({
      imports: [LoginPage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    const fixture = TestBed.createComponent(LoginPage);
    fixture.detectChanges();
    return fixture;
  }

  it('toggles password visibility', async () => {
    const fixture = await createPage();
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    const visibilityButton = fixture.nativeElement.querySelector(
      '.visibility-button',
    ) as HTMLButtonElement;

    expect(password.type).toBe('password');
    visibilityButton.click();
    fixture.detectChanges();
    expect(password.type).toBe('text');
  });

  it('posts login credentials and navigates to an authorized page', async () => {
    const fixture = await createPage();
    const http = TestBed.inject(HttpTestingController);
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    fixture.componentInstance.username = 'trung';
    fixture.componentInstance.password = '123456';
    fixture.componentInstance.rememberMe = true;
    fixture.componentInstance.submitLogin();
    const request = http.expectOne('/api/auth/login');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ username: 'trung', password: '123456' });

    request.flush({
      accessToken: 'test-token',
      expiresAt: '2026-10-08T08:00:00Z',
      user: {
        userId: 'trung',
        userFamilyName: null,
        userFirstName: null,
        roles: ['USER'],
        categories: [{ categoryId: 'C000000005', categoryName: 'LOGISTICS' }],
      },
    });
    fixture.detectChanges();

    expect(navigateSpy).toHaveBeenCalledWith('/home');
    expect(localStorage.getItem('ojitex.auth')).toContain('test-token');
    expect(sessionStorage.getItem('ojitex.auth')).toBeNull();
    http.verify();
  });

  it('shows a useful message when the credentials are rejected', async () => {
    const fixture = await createPage();
    const http = TestBed.inject(HttpTestingController);

    fixture.componentInstance.username = 'trung';
    fixture.componentInstance.password = 'wrong-password';
    fixture.componentInstance.submitLogin();
    http.expectOne('/api/auth/login').flush(
      { message: 'Invalid credentials' },
      { status: 401, statusText: 'Unauthorized' },
    );
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('[role="status"]').textContent).toContain(
      'The username or password is incorrect',
    );
    http.verify();
  });
});
