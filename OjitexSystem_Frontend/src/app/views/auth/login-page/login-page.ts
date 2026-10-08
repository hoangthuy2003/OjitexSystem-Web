import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule],
  selector: 'app-login-page',
  styleUrl: './login-page.scss',
  templateUrl: './login-page.html',
})
export class LoginPage {
  username = '';
  password = '';
  rememberMe = false;
  showPassword = false;
  statusMessage = '';

  submitLogin(): void {
    this.statusMessage = 'Chức năng đăng nhập chưa được kết nối với máy chủ.';
  }
}
