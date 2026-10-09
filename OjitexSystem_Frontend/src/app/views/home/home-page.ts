import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

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
}
