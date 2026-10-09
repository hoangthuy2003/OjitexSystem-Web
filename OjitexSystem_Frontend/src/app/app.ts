import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnDestroy, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { TimeoutError, timeout } from 'rxjs';
import { AuthService } from './services/auth.service';

@Component({
  imports: [FormsModule, RouterLink, RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnDestroy {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private passwordNoticeTimer: ReturnType<typeof setTimeout> | undefined;

  changePasswordOpen = false;
  changingPassword = false;
  currentPassword = '';
  newPassword = '';
  confirmNewPassword = '';
  passwordChangeError = '';
  passwordChangeNotice = '';

  get isAuthenticated(): boolean {
    return this.authService.accessToken !== null;
  }

  get currentUserName(): string {
    const user = this.authService.currentUser;
    return [user?.userFamilyName, user?.userFirstName].filter(Boolean).join(' ') || user?.userId || '';
  }

  ngOnDestroy(): void {
    this.clearPasswordNoticeTimer();
  }

  logout(): void {
    this.authService.clearSession();
    void this.router.navigateByUrl('/login');
  }

  openChangePassword(): void {
    this.currentPassword = '';
    this.newPassword = '';
    this.confirmNewPassword = '';
    this.passwordChangeError = '';
    this.clearPasswordNoticeTimer();
    this.passwordChangeNotice = '';
    this.changePasswordOpen = true;
  }

  closeChangePassword(): void {
    if (this.changingPassword) {
      return;
    }

    this.changePasswordOpen = false;
    this.passwordChangeError = '';
  }

  closeChangePasswordFromBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.closeChangePassword();
    }
  }

  submitChangePassword(): void {
    if (this.changingPassword) {
      return;
    }

    if (this.newPassword.length < 8) {
      this.passwordChangeError = 'New password must be at least 8 characters long.';
      return;
    }

    if (this.newPassword !== this.confirmNewPassword) {
      this.passwordChangeError = 'The new password and confirmation do not match.';
      return;
    }

    this.changingPassword = true;
    this.passwordChangeError = '';
    this.authService.changePassword(this.currentPassword, this.newPassword)
      .pipe(timeout({ first: 15000 }))
      .subscribe({
        next: () => {
          this.changingPassword = false;
          this.changePasswordOpen = false;
          this.currentPassword = '';
          this.newPassword = '';
          this.confirmNewPassword = '';
          this.passwordChangeNotice = 'Password changed successfully.';
          this.changeDetector.markForCheck();
          this.passwordNoticeTimer = setTimeout(() => {
            this.passwordChangeNotice = '';
            this.passwordNoticeTimer = undefined;
            this.changeDetector.markForCheck();
          }, 3000);
        },
        error: (error: HttpErrorResponse | TimeoutError) => {
          this.changingPassword = false;
          this.passwordChangeError = this.getChangePasswordError(error);
          this.changeDetector.markForCheck();
        },
      });
  }

  private getChangePasswordError(error: HttpErrorResponse | TimeoutError): string {
    if (error instanceof TimeoutError || error.status === 0) {
      return 'Unable to reach the server. Please try again.';
    }

    if (error.status === 401 || error.status === 403) {
      this.logout();
      return 'Your session has expired. Please sign in again.';
    }

    if (typeof error.error?.message === 'string' && error.error.message.trim()) {
      return error.error.message;
    }

    return 'Unable to change the password. Please try again.';
  }

  private clearPasswordNoticeTimer(): void {
    if (this.passwordNoticeTimer !== undefined) {
      clearTimeout(this.passwordNoticeTimer);
      this.passwordNoticeTimer = undefined;
    }
  }
}
