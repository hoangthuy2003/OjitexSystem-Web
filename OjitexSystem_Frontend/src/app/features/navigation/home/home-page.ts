import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { DEPARTMENTS, Department } from '../department-catalog';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  imports: [RouterLink],
  selector: 'app-home-page',
  styleUrl: './home-page.scss',
  templateUrl: './home-page.html',
})
export class HomePage {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  adminAccessDialogOpen = false;

  readonly departments = DEPARTMENTS;

  openDepartment(event: MouseEvent, department: Department): void {
    if (department.slug !== 'admin') {
      return;
    }

    event.preventDefault();
    if (!this.authService.isAdmin) {
      this.adminAccessDialogOpen = true;
      return;
    }

    void this.router.navigateByUrl('/admin/dashboard');
  }

  closeAdminAccessDialog(): void {
    this.adminAccessDialogOpen = false;
  }

  closeAdminAccessDialogFromBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.closeAdminAccessDialog();
    }
  }
}
