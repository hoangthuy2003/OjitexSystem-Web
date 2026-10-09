import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AdminUser, UserForm, UserRole } from '../../../interfaces/admin-user.interface';
import { Category } from '../../../interfaces/auth.interface';
import { AdminUserService } from '../../../services/admin-user.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  imports: [DatePipe, FormsModule],
  selector: 'app-admin-users-page',
  styleUrl: './admin-users-page.scss',
  templateUrl: './admin-users-page.html',
})
export class AdminUsersPage implements OnInit {
  private readonly adminUserService = inject(AdminUserService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  users: AdminUser[] = [];
  roles: UserRole[] = [];
  categories: Category[] = [];
  form: UserForm = this.emptyForm();
  searchTerm = '';
  formOpen = false;
  editing = false;
  loading = false;
  saving = false;
  pageError = '';
  formError = '';
  notice = '';

  get hasStockAccess(): boolean {
    return this.authService.hasCategory('C000000005');
  }

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

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.pageError = '';
    forkJoin({
      users: this.adminUserService.getUsers(),
      roles: this.adminUserService.getRoles(),
      categories: this.adminUserService.getCategories(),
    }).subscribe({
      next: ({ users, roles, categories }) => {
        this.users = users;
        this.roles = roles;
        this.categories = categories;
        this.loading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.pageError = this.getErrorMessage(error);
      },
    });
  }

  startCreate(): void {
    this.form = this.emptyForm();
    this.editing = false;
    this.formOpen = true;
    this.formError = '';
    this.notice = '';
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
    this.notice = '';
  }

  cancelEdit(): void {
    this.form = this.emptyForm();
    this.editing = false;
    this.formOpen = false;
    this.formError = '';
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
      ? this.adminUserService.updateUser(this.form)
      : this.adminUserService.createUser(this.form);

    request.subscribe({
      next: (user) => {
        this.saving = false;
        this.notice = this.editing
          ? `User account ${user.userId} was updated.`
          : `User account ${user.userId} was created. The default password is 123456.`;
        this.cancelEdit();
        this.loadData();
      },
      error: (error: HttpErrorResponse) => {
        this.saving = false;
        this.formError = this.getErrorMessage(error);
      },
    });
  }

  resetPassword(user: AdminUser): void {
    if (!window.confirm(`Reset the password for "${user.userId}" to 123456?`)) {
      return;
    }

    this.adminUserService.resetPassword(user.userId).subscribe({
      next: () => {
        this.notice = `The password for ${user.userId} was reset to the default password (123456).`;
        this.pageError = '';
      },
      error: (error: HttpErrorResponse) => {
        this.pageError = this.getErrorMessage(error);
      },
    });
  }

  deleteUser(user: AdminUser): void {
    if (!window.confirm(`Deactivate the user account "${user.userId}"?`)) {
      return;
    }

    this.adminUserService.deleteUser(user.userId).subscribe({
      next: () => {
        this.notice = `User account ${user.userId} was deactivated.`;
        this.pageError = '';
        this.loadData();
      },
      error: (error: HttpErrorResponse) => {
        this.pageError = this.getErrorMessage(error);
      },
    });
  }

  logout(): void {
    this.authService.clearSession();
    void this.router.navigateByUrl('/login');
  }

  openStockPage(): void {
    void this.router.navigateByUrl('/logistic/current-stock');
  }

  displayName(user: AdminUser): string {
    return [user.userFamilyName, user.userFirstName].filter(Boolean).join(' ') || '—';
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

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Unable to connect to the server. Check the backend and try again.';
    }

    if (error.status === 401 || error.status === 403) {
      this.authService.clearSession();
      void this.router.navigateByUrl('/login');
      return 'Your session has expired or your account no longer has administrator access.';
    }

    return 'Unable to complete the request. Please try again.';
  }
}
