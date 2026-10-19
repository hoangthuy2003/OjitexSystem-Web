import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, inject, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TimeoutError } from 'rxjs';
import { AuthService } from '../../../core/auth/auth.service';
import { AuthSessionError } from '../../../core/auth/auth-session.store';

@Component({
  imports: [FormsModule],
  selector: 'app-login-page',
  styleUrl: './login-page.scss',
  templateUrl: './login-page.html',
})
export class LoginPage {
  private readonly destroyRef = inject(DestroyRef);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);

  username = '';
  password = '';
  rememberMe = true;
  showPassword = false;
  statusMessage = '';
  statusType: 'error' | 'success' = 'error';
  isSubmitting = false;

  submitLogin(): void {
    if (this.isSubmitting) {
      return;
    }

    this.statusMessage = '';
    this.isSubmitting = true;
    this.authService
      .login(this.username.trim(), this.password, this.rememberMe)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.changeDetector.markForCheck();
          void this.router.navigateByUrl('/home', { replaceUrl: true });
        },
        error: (error: HttpErrorResponse | TimeoutError | AuthSessionError) => {
          this.isSubmitting = false;
          this.statusType = 'error';
          this.statusMessage = this.getLoginErrorMessage(error);
          this.changeDetector.markForCheck();
        },
      });
  }

  private getLoginErrorMessage(error: HttpErrorResponse | TimeoutError | AuthSessionError): string {
    if (error instanceof AuthSessionError) {
      return error.message;
    }
    if (error instanceof TimeoutError) {
      return 'The sign-in request timed out. Please try again.';
    }

    if (error.status === 0) {
      return 'Unable to connect to the server. Check the backend and try again.';
    }

    if (error.status === 401) {
      return 'The username or password is incorrect. If you forgot your password, please contact the administrator.';
    }

    return 'Sign-in failed due to a server error. Please try again later.';
  }
}
