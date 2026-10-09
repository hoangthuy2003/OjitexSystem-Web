import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
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
    this.authService.login(this.username.trim(), this.password, this.rememberMe).subscribe({
      next: () => {
        this.isSubmitting = false;
        void this.router.navigateByUrl('/home');
      },
      error: (error: HttpErrorResponse) => {
        this.isSubmitting = false;
        this.statusType = 'error';
        this.statusMessage = this.getLoginErrorMessage(error);
      },
    });
  }

  private getLoginErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Unable to connect to the server. Check the backend and try again.';
    }

    if (error.status === 401) {
      return 'The username or password is incorrect, or the account is locked.';
    }

    return 'Sign-in failed due to a server error. Please try again later.';
  }
}
