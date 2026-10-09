import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AdminUser } from '../../../interfaces/admin-dashboard.interface';
import { authInterceptor } from '../../../services/auth.interceptor';
import { AdminDashboardPage } from './admin-dashboard-page';

describe('AdminDashboardPage', () => {
  const user: AdminUser = {
    userId: 'trung',
    userFamilyName: 'Nguyen',
    userFirstName: 'Trung',
    isLocked: false,
    lastLoginDate: null,
    roles: ['USER'],
    categories: [{ categoryId: 'C000000005', categoryName: 'LOGISTICS' }],
    directCategoryIds: [],
  };

  async function createPage(failRoleRequest = false) {
    sessionStorage.setItem(
      'ojitex.auth',
      JSON.stringify({
        accessToken: 'test-token',
        expiresAt: '2026-10-08T08:00:00Z',
        user: {
          userId: 'admin',
          userFamilyName: null,
          userFirstName: null,
          roles: ['ADMIN'],
          categories: [],
        },
      }),
    );

    await TestBed.configureTestingModule({
      imports: [AdminDashboardPage],
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(AdminDashboardPage);
    const http = TestBed.inject(HttpTestingController);
    fixture.detectChanges();

    const usersRequest = http.expectOne('/api/admin/dashboard');
    expect(usersRequest.request.headers.get('Authorization')).toBe('Bearer test-token');
    usersRequest.flush([user]);
    const rolesRequest = http.expectOne('/api/admin/dashboard/roles');
    if (failRoleRequest) {
      rolesRequest.flush('Role lookup failed.', { status: 500, statusText: 'Server Error' });
    } else {
      rolesRequest.flush([
        { role: 'USER', description: 'User' },
        { role: 'ADMIN', description: 'Admin' },
      ]);
    }
    http.expectOne('/api/admin/dashboard/categories').flush([
      { categoryId: 'C000000005', categoryName: 'LOGISTICS' },
    ]);
    await fixture.whenStable();

    return { fixture, http };
  }

  it('loads the user list and opens the create form', async () => {
    const { fixture, http } = await createPage();

    expect(fixture.nativeElement.textContent).toContain('trung');
    expect(fixture.nativeElement.textContent).not.toContain('Category Access');
    expect(fixture.nativeElement.querySelectorAll('tbody tr td')).toHaveLength(6);
    expect(fixture.nativeElement.querySelector('thead').textContent).toContain('Action');
    expect(fixture.nativeElement.querySelectorAll('.row-actions .action-button')).toHaveLength(3);
    expect(getComputedStyle(fixture.nativeElement.querySelector('table')).textAlign).toBe('center');
    expect(getComputedStyle(fixture.nativeElement.querySelector('.row-actions')).flexWrap).toBe(
      'nowrap',
    );
    expect(getComputedStyle(fixture.nativeElement.querySelector('.delete-action')).whiteSpace).toBe(
      'nowrap',
    );

    const createButton = fixture.nativeElement.querySelector(
      '.list-actions .primary',
    ) as HTMLButtonElement;
    createButton.click();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('#editor-heading').textContent).toContain(
      'Create User',
    );
    expect(fixture.nativeElement.querySelector('[role="dialog"][aria-modal="true"]')).not.toBeNull();
    http.verify();
  });

  it('renders the users as soon as the API responds without another user interaction', async () => {
    const { fixture, http } = await createPage();

    expect(fixture.nativeElement.textContent).toContain('trung');
    expect(fixture.nativeElement.textContent).not.toContain('Loading user data...');
    http.verify();
  });

  it('keeps the user list visible if the roles lookup fails', async () => {
    const { fixture, http } = await createPage(true);

    expect(fixture.nativeElement.textContent).toContain('trung');
    expect(fixture.componentInstance.pageError).toContain('Unable to load roles');
    http.verify();
  });

  it('sends user profile and selected access when creating an account', async () => {
    const { fixture, http } = await createPage();
    const page = fixture.componentInstance;
    page.startCreate();
    page.form.userId = 'new-user';
    page.form.userFamilyName = 'Tran';
    page.form.userFirstName = 'An';
    page.form.roles = ['USER'];
    page.form.categoryIds = ['C000000005'];

    page.saveUser();
    const createRequest = http.expectOne('/api/admin/dashboard');
    expect(createRequest.request.method).toBe('POST');
    expect(createRequest.request.body).toEqual({
      userId: 'new-user',
      userFamilyName: 'Tran',
      userFirstName: 'An',
      roles: ['USER'],
      categoryIds: ['C000000005'],
    });
    createRequest.flush({ ...user, userId: 'new-user' });

    http.expectOne('/api/admin/dashboard').flush([{ ...user, userId: 'new-user' }]);
    http.expectOne('/api/admin/dashboard/roles').flush([{ role: 'USER', description: 'User' }]);
    http.expectOne('/api/admin/dashboard/categories').flush([
      { categoryId: 'C000000005', categoryName: 'LOGISTICS' },
    ]);
    fixture.detectChanges();

    expect(page.formOpen).toBe(false);
    expect(page.notice).toContain('new-user');
    http.verify();
  });

  it('shows success feedback as a top-right toast and dismisses it after three seconds', async () => {
    const { fixture, http } = await createPage();
    vi.useFakeTimers();

    try {
      const page = fixture.componentInstance;
      page.startCreate();
      page.form.userId = 'new-user';
      page.saveUser();

      http.expectOne('/api/admin/dashboard').flush({ ...user, userId: 'new-user' });
      fixture.detectChanges();

      const toast = fixture.nativeElement.querySelector('.toast') as HTMLElement;
      expect(toast.textContent).toContain('new-user');
      expect(toast.getAttribute('role')).toBe('status');

      vi.advanceTimersByTime(3000);
      fixture.detectChanges();

      expect(fixture.nativeElement.querySelector('.toast')).toBeNull();
      http.expectOne('/api/admin/dashboard').flush([user]);
      http.expectOne('/api/admin/dashboard/roles').flush([]);
      http.expectOne('/api/admin/dashboard/categories').flush([]);
      http.verify();
    } finally {
      vi.useRealTimers();
    }
  });

  it('resets a user password through the admin API', async () => {
    const { fixture, http } = await createPage();
    const page = fixture.componentInstance;

    page.resetPassword(user);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="dialog"]').textContent).toContain('123456');
    http.expectNone('/api/admin/dashboard/trung/reset-password');

    page.confirmPendingAction();
    const resetRequest = http.expectOne('/api/admin/dashboard/trung/reset-password');
    expect(resetRequest.request.method).toBe('POST');
    resetRequest.flush({ message: 'Password reset.' });

    expect(page.notice).toContain('trung');
    expect(page.resettingUserIds.has('trung')).toBe(false);
    http.expectOne('/api/admin/dashboard').flush([user]);
    http.expectOne('/api/admin/dashboard/roles').flush([]);
    http.expectOne('/api/admin/dashboard/categories').flush([]);
    http.verify();
  });

  it('clears the reset indicator and reports an API failure', async () => {
    const { fixture, http } = await createPage();
    const page = fixture.componentInstance;

    page.resetPassword(user);
    page.confirmPendingAction();
    http.expectOne('/api/admin/dashboard/trung/reset-password').flush(
      { message: 'Reset failed.' },
      { status: 500, statusText: 'Server Error' },
    );

    expect(page.resettingUserIds.has('trung')).toBe(false);
    expect(page.pageError).toContain('Reset failed.');
    http.verify();
  });

  it('does not perform a user action when its confirmation is cancelled', async () => {
    const { fixture, http } = await createPage();
    const page = fixture.componentInstance;

    page.deleteUser(user);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="dialog"]').textContent).toContain(
      'cannot be undone',
    );

    page.cancelPendingAction();
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="dialog"]')).toBeNull();
    expect(page.pendingConfirmation).toBeNull();
    http.expectNone('/api/admin/dashboard/trung');
    http.verify();
  });

  it('deletes a user only after the confirmation is accepted', async () => {
    const { fixture, http } = await createPage();
    const page = fixture.componentInstance;

    page.deleteUser(user);
    http.expectNone('/api/admin/dashboard/trung');
    page.confirmPendingAction();

    const deleteRequest = http.expectOne('/api/admin/dashboard/trung');
    expect(deleteRequest.request.method).toBe('DELETE');
    deleteRequest.flush(null);

    expect(page.notice).toContain('trung');
    http.expectOne('/api/admin/dashboard').flush([]);
    http.expectOne('/api/admin/dashboard/roles').flush([]);
    http.expectOne('/api/admin/dashboard/categories').flush([]);
    http.verify();
  });

  it('shows no more than eight users per page', async () => {
    sessionStorage.setItem(
      'ojitex.auth',
      JSON.stringify({
        accessToken: 'test-token',
        expiresAt: '2026-10-08T08:00:00Z',
        user: {
          userId: 'admin',
          userFamilyName: null,
          userFirstName: null,
          roles: ['ADMIN'],
          categories: [],
        },
      }),
    );

    await TestBed.configureTestingModule({
      imports: [AdminDashboardPage],
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(AdminDashboardPage);
    const http = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
    http.expectOne('/api/admin/dashboard').flush(
      Array.from({ length: 10 }, (_, index) => ({ ...user, userId: `user-${index + 1}` })),
    );
    http.expectOne('/api/admin/dashboard/roles').flush([]);
    http.expectOne('/api/admin/dashboard/categories').flush([]);
    await fixture.whenStable();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelectorAll('tbody tr')).toHaveLength(8);
    fixture.nativeElement.querySelector('[aria-label="Next page"]').click();
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelectorAll('tbody tr')).toHaveLength(2);
    http.verify();
  });
});
