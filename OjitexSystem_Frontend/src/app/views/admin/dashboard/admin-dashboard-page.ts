import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TimeoutError, timeout } from 'rxjs';
import { AdminUser, UserForm, UserRole } from '../../../interfaces/admin-dashboard.interface';
import { Category } from '../../../interfaces/auth.interface';
import { AdminDashboardService } from '../../../services/admin-dashboard.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  imports: [DatePipe, FormsModule],
  selector: 'app-admin-dashboard-page',
  styleUrl: './admin-dashboard-page.scss',
  templateUrl: './admin-dashboard-page.html',
})
export class AdminDashboardPage implements OnDestroy, OnInit {
  private readonly adminDashboardService = inject(AdminDashboardService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly requestTimeoutMs = 15000;
  private loadRequestId = 0;
  private noticeTimer: ReturnType<typeof setTimeout> | undefined;

  users: AdminUser[] = [];
  roles: UserRole[] = [];
  categories: Category[] = [];
  readonly pageSize = 8;
  form: UserForm = this.emptyForm();
  searchTerm = '';
  currentPage = 1;
  formOpen = false;
  editing = false;
  loading = false;
  usersLoadFailed = false;
  saving = false;
  readonly resettingUserIds = new Set<string>();
  pageError = '';
  formError = '';
  notice = '';

  get filteredUsers(): AdminUser[] {
    const search = this.searchTerm.trim().toLocaleLowerCase();
    if (!search) {
      return this.users;
    }

    return this.users.filter((user) =>
      [user.userId, user.userFamilyName, user.userFirstName, ...user.roles]
        .filter((value): value is string => value !== null)
        .some((value) => value.toLocaleLowerCase().includes(search)),
    );
  }

  get pageCount(): number {
    return Math.max(1, Math.ceil(this.filteredUsers.length / this.pageSize));
  }

  get paginatedUsers(): AdminUser[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredUsers.slice(start, start + this.pageSize);
  }

  get pageStart(): number {
    return this.filteredUsers.length === 0 ? 0 : (this.currentPage - 1) * this.pageSize + 1;
  }

  get pageEnd(): number {
    return Math.min(this.currentPage * this.pageSize, this.filteredUsers.length);
  }

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.clearNoticeTimer();
  }

  loadData(): void {
    const requestId = ++this.loadRequestId;
    this.loading = true;
    this.usersLoadFailed = false;
    this.pageError = '';

    this.adminDashboardService.getUsers().pipe(
      timeout({ first: this.requestTimeoutMs }),
    ).subscribe({
      next: (users) => {
        if (requestId !== this.loadRequestId) return;
        this.users = users;
        this.loading = false;
        this.goToPage(this.currentPage);
        this.changeDetector.markForCheck();
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        if (requestId !== this.loadRequestId) return;
        this.loading = false;
        this.usersLoadFailed = true;
        this.appendLoadError('Unable to load user accounts', error);
        this.changeDetector.markForCheck();
      },
    });

    this.adminDashboardService.getRoles().pipe(
      timeout({ first: this.requestTimeoutMs }),
    ).subscribe({
      next: (roles) => {
        if (requestId === this.loadRequestId) {
          this.roles = roles;
          this.changeDetector.markForCheck();
        }
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        if (requestId === this.loadRequestId) {
          this.appendLoadError('Unable to load roles', error);
          this.changeDetector.markForCheck();
        }
      },
    });

    this.adminDashboardService.getCategories().pipe(
      timeout({ first: this.requestTimeoutMs }),
    ).subscribe({
      next: (categories) => {
        if (requestId === this.loadRequestId) {
          this.categories = categories;
          this.changeDetector.markForCheck();
        }
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        if (requestId === this.loadRequestId) {
          this.appendLoadError('Unable to load page access categories', error);
          this.changeDetector.markForCheck();
        }
      },
    });
  }

  goToPage(page: number): void {
    this.currentPage = Math.min(Math.max(1, page), this.pageCount);
  }

  startCreate(): void {
    this.form = this.emptyForm();
    this.editing = false;
    this.formOpen = true;
    this.formError = '';
    this.clearNotice();
  }

  startEdit(user: AdminUser): void {
    this.form = {
      userId: user.userId,
      userFamilyName: user.userFamilyName ?? '',
      userFirstName: user.userFirstName ?? '',
      roles: [...user.roles],
      categoryIds: [...user.directCategoryIds],
    };
    this.editing = true;
    this.formOpen = true;
    this.formError = '';
    this.clearNotice();
  }

  cancelEdit(): void {
    this.form = this.emptyForm();
    this.editing = false;
    this.formOpen = false;
    this.formError = '';
  }

  closeFormFromBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.cancelEdit();
    }
  }

  toggleRole(role: string, checked: boolean): void {
    this.form.roles = this.toggleSelection(this.form.roles, role, checked);
  }

  toggleCategory(categoryId: string, checked: boolean): void {
    this.form.categoryIds = this.toggleSelection(this.form.categoryIds, categoryId, checked);
  }

  saveUser(): void {
    if (this.saving) {
      return;
    }

    this.saving = true;
    this.formError = '';
    const request = this.editing
      ? this.adminDashboardService.updateUser(this.form)
      : this.adminDashboardService.createUser(this.form);

    request.pipe(timeout({ first: this.requestTimeoutMs })).subscribe({
      next: (user) => {
        this.saving = false;
        this.showNotice(this.editing
          ? `User account ${user.userId} was updated.`
          : `User account ${user.userId} was created. The default password is 123456.`);
        this.cancelEdit();
        this.changeDetector.markForCheck();
        this.loadData();
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        this.saving = false;
        this.formError = this.getErrorMessage(error);
        this.changeDetector.markForCheck();
      },
    });
  }

  resetPassword(user: AdminUser): void {
    if (this.resettingUserIds.has(user.userId)) {
      return;
    }

    if (!window.confirm(`Reset the password for "${user.userId}" to 123456?`)) {
      return;
    }

    this.resettingUserIds.add(user.userId);
    this.clearNotice();
    this.pageError = '';
    this.adminDashboardService.resetPassword(user.userId).pipe(
      timeout({ first: this.requestTimeoutMs }),
    ).subscribe({
      next: () => {
        this.showNotice(`The password for ${user.userId} was reset to the default password (123456).`);
        this.resettingUserIds.delete(user.userId);
        this.changeDetector.markForCheck();
        this.loadData();
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        this.pageError = this.getErrorMessage(error);
        this.resettingUserIds.delete(user.userId);
        this.changeDetector.markForCheck();
      },
    });
  }

  deleteUser(user: AdminUser): void {
    if (!window.confirm(`Deactivate the user account "${user.userId}"?`)) {
      return;
    }

    this.adminDashboardService.deleteUser(user.userId).pipe(
      timeout({ first: this.requestTimeoutMs }),
    ).subscribe({
      next: () => {
        this.showNotice(`User account ${user.userId} was deactivated.`);
        this.pageError = '';
        this.changeDetector.markForCheck();
        this.loadData();
      },
      error: (error: HttpErrorResponse | TimeoutError) => {
        this.pageError = this.getErrorMessage(error);
        this.changeDetector.markForCheck();
      },
    });
  }

  logout(): void {
    this.authService.clearSession();
    void this.router.navigateByUrl('/login');
  }

  displayName(user: AdminUser): string {
    return [user.userFamilyName, user.userFirstName].filter(Boolean).join(' ') || '—';
  }

  private showNotice(message: string): void {
    this.clearNoticeTimer();
    this.notice = message;
    this.changeDetector.markForCheck();
    this.noticeTimer = setTimeout(() => {
      this.notice = '';
      this.noticeTimer = undefined;
      this.changeDetector.markForCheck();
    }, 3000);
  }

  private clearNotice(): void {
    this.clearNoticeTimer();
    this.notice = '';
  }

  private clearNoticeTimer(): void {
    if (this.noticeTimer !== undefined) {
      clearTimeout(this.noticeTimer);
      this.noticeTimer = undefined;
    }
  }

  private emptyForm(): UserForm {
    return {
      userId: '',
      userFamilyName: '',
      userFirstName: '',
      roles: [],
      categoryIds: [],
    };
  }

  private toggleSelection(values: string[], value: string, checked: boolean): string[] {
    if (checked) {
      return values.includes(value) ? values : [...values, value];
    }

    return values.filter((item) => item !== value);
  }

  private appendLoadError(context: string, error: HttpErrorResponse | TimeoutError): void {
    const message = `${context}: ${this.getErrorMessage(error)}`;
    this.pageError = [this.pageError, message].filter(Boolean).join(' ');
  }

  private getErrorMessage(error: HttpErrorResponse | TimeoutError): string {
    if (error instanceof TimeoutError) {
      return 'The server took too long to respond. Please try again.';
    }

    if (error.status === 0) {
      return 'Unable to connect to the server. Check the backend and try again.';
    }

    if (error.status === 401 || error.status === 403) {
      this.authService.clearSession();
      void this.router.navigateByUrl('/login');
      return 'Your session has expired or your account no longer has administrator access.';
    }

    if (typeof error.error?.message === 'string' && error.error.message.trim()) {
      return error.error.message;
    }

    return 'Unable to complete the request. Please try again.';
  }
}
