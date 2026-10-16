export interface Department {
  name: string;
  slug: string;
}

export interface ReportItem {
  name: string;
  route?: string;
}

export const DEPARTMENTS: Department[] = [
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

// Entries without a route describe planned features and remain disabled in the UI.
export const REPORT_ITEMS: ReportItem[] = [
  { name: 'Current Stock', route: '/logistic/current-stock' },
  { name: 'Work Progress' },
  { name: 'Last Stock', route: '/logistic/last-stock' },
  { name: 'Management Loss' },
  { name: 'Monthly Stock', route: '/logistic/monthly-stock' },
  { name: 'FFG' },
];

export const FACTORY_ITEMS: (ReportItem & { detail: string })[] = [
  { name: 'Corrugator', detail: 'Online', route: '/factory/corrugator-online' },
  { name: 'FDC', detail: 'Online', route: '/factory/fdc-online' },
  { name: 'Corrugator', detail: 'History' },
  { name: 'Converting Label', detail: 'Reprint' },
  { name: 'EVOL', detail: 'Online', route: '/factory/evol-online' },
  { name: 'FAP', detail: 'Online', route: '/factory/fap-online' },
  { name: 'Converting', detail: 'History' },
  { name: 'FPS', detail: 'Online', route: '/factory/fps-online' },
  { name: 'FG', detail: 'Online', route: '/factory/fg-online' },
];
