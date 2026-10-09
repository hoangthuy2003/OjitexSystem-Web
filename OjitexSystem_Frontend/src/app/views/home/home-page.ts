import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface Department {
  name: string;
  slug: string;
}

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

  readonly departments: Department[] = [
    { name: 'SALES', slug: 'sales' },
    { name: 'CS', slug: 'cs' },
    { name: 'PLAN', slug: 'plan' },
    { name: 'PRODUCTION', slug: 'production' },
    { name: 'LOGISTICS', slug: 'logistics' },
    { name: 'QC', slug: 'qc' },
    { name: 'ACCOUNTANT', slug: 'accountant' },
    { name: 'MATERIAL', slug: 'material' },
    { name: 'GA', slug: 'ga' },
    { name: 'FACTORY', slug: 'factory' },
    { name: 'STOCK', slug: 'stock' },
    { name: 'REPORT', slug: 'report' },
    { name: 'ADMIN', slug: 'admin' },
  ];

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
