import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  imports: [RouterLink, RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  get isAuthenticated(): boolean {
    return this.authService.accessToken !== null;
  }

  get currentUserName(): string {
    const user = this.authService.currentUser;
    return [user?.userFamilyName, user?.userFirstName].filter(Boolean).join(' ') || user?.userId || '';
  }

  logout(): void {
    this.authService.clearSession();
    void this.router.navigateByUrl('/login');
  }
}
