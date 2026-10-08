import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AdminUser } from '../../../interfaces/admin-user.interface';
import { authInterceptor } from '../../../services/auth.interceptor';
import { AdminUsersPage } from './admin-users-page';

describe('AdminUsersPage', () => {
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

  async function createPage() {
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
      imports: [AdminUsersPage],
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(AdminUsersPage);
    const http = TestBed.inject(HttpTestingController);
    fixture.detectChanges();

    const usersRequest = http.expectOne('/api/admin/users');
    expect(usersRequest.request.headers.get('Authorization')).toBe('Bearer test-token');
    usersRequest.flush([user]);
    http.expectOne('/api/admin/users/roles').flush([
      { role: 'USER', description: 'User' },
      { role: 'ADMIN', description: 'Admin' },
    ]);
    http.expectOne('/api/admin/users/categories').flush([
      { categoryId: 'C000000005', categoryName: 'LOGISTICS' },
    ]);
    await fixture.whenStable();
    fixture.detectChanges();

    return { fixture, http };
  }

  it('loads the user list and opens the create form', async () => {
    const { fixture, http } = await createPage();

    expect(fixture.nativeElement.textContent).toContain('trung');
    expect(fixture.nativeElement.textContent).toContain('LOGISTICS');

    const createButton = fixture.nativeElement.querySelector(
      '.list-actions .primary',
    ) as HTMLButtonElement;
    createButton.click();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('#editor-heading').textContent).toContain(
      'Tạo tài khoản',
    );
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
    const createRequest = http.expectOne('/api/admin/users');
    expect(createRequest.request.method).toBe('POST');
    expect(createRequest.request.body).toEqual({
      userId: 'new-user',
      userFamilyName: 'Tran',
      userFirstName: 'An',
      roles: ['USER'],
      categoryIds: ['C000000005'],
    });
    createRequest.flush({ ...user, userId: 'new-user' });

    http.expectOne('/api/admin/users').flush([{ ...user, userId: 'new-user' }]);
    http.expectOne('/api/admin/users/roles').flush([{ role: 'USER', description: 'User' }]);
    http.expectOne('/api/admin/users/categories').flush([
      { categoryId: 'C000000005', categoryName: 'LOGISTICS' },
    ]);
    fixture.detectChanges();

    expect(page.formOpen).toBe(false);
    expect(page.notice).toContain('new-user');
    http.verify();
  });
});
