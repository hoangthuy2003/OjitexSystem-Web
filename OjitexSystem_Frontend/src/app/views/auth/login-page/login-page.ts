import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TimeoutError, timeout } from 'rxjs';
import { AuthService } from '../../../services/auth.service';

@Component({
  imports: [FormsModule],
  selector: 'app-login-page',
  styleUrl: './login-page.scss',
  templateUrl: './login-page.html',
})
export class LoginPage {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);

  username = '';
  password = '';
  rememberMe = false;
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
    this.authService.login(this.username.trim(), this.password, this.rememberMe)
      .pipe(timeout({ first: 15000 }))
      .subscribe({
        next: () => {
          this.isSubmitting = false;
          this.changeDetector.markForCheck();
          void this.router.navigateByUrl('/home', { replaceUrl: true });
        },
        error: (error: HttpErrorResponse | TimeoutError) => {
          this.isSubmitting = false;
          this.statusType = 'error';
          this.statusMessage = this.getLoginErrorMessage(error);
          this.changeDetector.markForCheck();
        },
      });
  }

  private getLoginErrorMessage(error: HttpErrorResponse | TimeoutError): string {
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
