import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { LogisticsMenuComponent } from '../../logistics/logistics-menu/logistics-menu.component';
import { FACTORY_ITEMS, REPORT_ITEMS, ReportItem } from '../department-catalog';

@Component({
  imports: [RouterLink, LogisticsMenuComponent],
  selector: 'app-department-page',
  styleUrl: './department-page.scss',
  templateUrl: './department-page.html',
})
export class DepartmentPage {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  get department(): string {
    return this.route.snapshot.paramMap.get('departmentId')?.toUpperCase() ?? 'DEPARTMENT';
  }

  get isReport(): boolean {
    return this.route.snapshot.paramMap.get('departmentId') === 'report';
  }

  protected get isFactory(): boolean {
    return this.route.snapshot.paramMap.get('departmentId') === 'factory';
  }

  protected get isLogistics(): boolean {
    return this.route.snapshot.paramMap.get('departmentId')?.toLowerCase() === 'logistics';
  }

  protected readonly factoryItems = FACTORY_ITEMS;

  readonly reportItems = REPORT_ITEMS;

  openReportItem(item: ReportItem): void {
    if (item.route) {
      void this.router.navigateByUrl(item.route);
    }
  }
}
