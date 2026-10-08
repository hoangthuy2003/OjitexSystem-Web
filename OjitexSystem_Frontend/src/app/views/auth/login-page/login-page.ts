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
      next: ({ user }) => {
        this.isSubmitting = false;
        if (user.roles.includes('ADMIN')) {
          void this.router.navigateByUrl('/admin/users');
          return;
        }

        if (user.categories.some((category) => category.categoryId === 'C000000005')) {
          void this.router.navigateByUrl('/logistic/current-stock');
          return;
        }

        this.statusType = 'success';
        this.statusMessage = 'Đăng nhập thành công nhưng tài khoản chưa được cấp quyền vào trang hiện có.';
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
      return 'Không kết nối được máy chủ. Vui lòng kiểm tra backend và thử lại.';
    }

    if (error.status === 401) {
      return 'Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đang bị khóa.';
    }

    return 'Đăng nhập thất bại do lỗi máy chủ. Vui lòng thử lại sau.';
  }
}
