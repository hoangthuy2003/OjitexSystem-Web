import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface ReportItem {
  name: string;
  route?: string;
}

@Component({
  imports: [RouterLink],
  selector: 'app-department-page',
  styleUrl: './department-page.scss',
  templateUrl: './department-page.html',
})
export class DepartmentPage {
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  get department(): string {
    return this.route.snapshot.paramMap.get('departmentId')?.toUpperCase() ?? 'DEPARTMENT';
  }

  get isReport(): boolean {
    return this.route.snapshot.paramMap.get('departmentId') === 'report';
  }

  get isAdmin(): boolean {
    return this.route.snapshot.paramMap.get('departmentId') === 'admin' && this.authService.isAdmin;
  }

  readonly reportItems: ReportItem[] = [
    { name: 'Current Stock', route: '/logistic/current-stock' },
    { name: 'Work Progress' },
    { name: 'Last Stock' },
    { name: 'Management Loss' },
    { name: 'Monthly Stock' },
    { name: 'FFG' },
  ];

  openReportItem(item: ReportItem): void {
    if (item.route) {
      void this.router.navigateByUrl(item.route);
    }
  }
}
