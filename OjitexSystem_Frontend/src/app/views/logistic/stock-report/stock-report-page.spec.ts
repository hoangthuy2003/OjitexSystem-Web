import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellDoubleClickedEvent } from 'ag-grid-community';
import {
  StockReportHistory,
  StockReportItem,
  StockReportKind,
} from '../../../interfaces/stock-report.interface';
import { routes } from '../../../app.routes';
import { logisticsAccessGuard } from '../../../guards/category-access.guard';
import { StockReportPage } from './stock-report-page';

const stock: StockReportItem = {
  id: 1,
  reportDate: '2023-07-31',
  productCode: 1040100,
  openingStock: 0,
  warehouseNg: 0,
  disposal: 0,
  repair: 0,
  production: 1420,
  delivery: 1420,
  closingStock: 0,
};

const history: StockReportHistory = {
  productCode: 1040100,
  productName: 'PIN-03A',
  customerName: 'MITSUBISHI',
  workingDayFrom: '2023-07-01',
  workingDayTo: '2023-07-31',
  items: [
    {
      id: 1,
      receiveNo: 0,
      productCode: 1040100,
      section: 'Logistics',
      inputStock: 0,
      outputStock: 1420,
      currentStock: 0,
      remark: 'Delivery Results',
      workDate: '2023-07-19',
      dateInput: '2023-07-19T16:39:00',
      userId: 'logivp04',
      userFirstName: 'Thu',
    },
  ],
};

describe.each<StockReportKind>(['last', 'monthly'])('%s stock report', (kind) => {
  const endpoint = kind === 'last' ? '/api/LastStock' : '/api/MonthlyStock';
  const report = {
    workingDayFrom: kind === 'last' ? '2026-09-01' : null,
    workingDayTo: kind === 'last' ? '2026-09-30' : null,
    items: [stock],
  };
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StockReportPage],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        { provide: ActivatedRoute, useValue: { snapshot: { data: { stockReport: kind } } } },
      ],
    }).compileComponents();
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function setup() {
    const fixture = TestBed.createComponent(StockReportPage);
    fixture.detectChanges();
    const dialog = fixture.nativeElement.querySelector('dialog') as HTMLDialogElement;
    dialog.showModal = vi.fn(() => dialog.setAttribute('open', ''));
    dialog.close = vi.fn(() => dialog.removeAttribute('open'));
    return { fixture, page: fixture.componentInstance, dialog };
  }

  function expectHistory(productCode = stock.productCode!, reportDate = stock.reportDate!) {
    return http.expectOne(
      (request) =>
        request.url === `${endpoint}/${productCode}/history` &&
        (kind === 'last'
          ? !request.params.has('reportDate')
          : request.params.get('reportDate') === reportDate),
    );
  }

  function doubleClick(page: StockReportPage, data = stock, field = 'closingStock') {
    page.onStockDoubleClicked({
      colDef: { field },
      data,
    } as CellDoubleClickedEvent<StockReportItem>);
  }

  it('starts idle and searches a prefix through the corresponding API', () => {
    const { fixture, page } = setup();
    expect(fixture.nativeElement.querySelector('h1').textContent).toContain(page.reportTitle);
    http.expectNone((request) => request.url.startsWith(endpoint));
    page.searchTerm = '104';
    page.searchByProductCode();
    http.expectOne(`${endpoint}/104`).flush(report);
    fixture.detectChanges();
    expect(page.stockItems).toEqual([stock]);
    if (kind === 'last')
      expect(fixture.nativeElement.querySelector('h1').textContent).toContain('Sep-2026');
    expect(page.columnDefs.some((column) => column.field === 'reportDate')).toBe(
      kind === 'monthly',
    );
  });

  it('opens exact product history from a zero stock cell and displays customer, dates and user name', () => {
    const { fixture, page, dialog } = setup();
    doubleClick(page);
    expect(dialog.open).toBe(true);
    expectHistory().flush(history);
    fixture.detectChanges();
    expect(dialog.textContent).toContain('MITSUBISHI');
    expect(dialog.textContent).toContain('PIN-03A');
    expect(dialog.textContent).toContain('Jul-01-2023');
    expect(dialog.textContent).toContain('Jul-31-2023');
    expect(dialog.querySelectorAll('tbody td')[0].textContent?.trim()).toBe('0');
    expect(dialog.querySelectorAll('tbody td')[5].textContent?.trim()).toBe('0');
    expect(dialog.querySelectorAll('tbody td')[9].textContent?.trim()).toBe('Thu');
    expect(dialog.textContent).not.toContain('logivp04');
  });

  it('ignores other columns and rows without a product code', () => {
    const { page, dialog } = setup();
    doubleClick(page, stock, 'openingStock');
    doubleClick(page, { ...stock, productCode: null });
    expect(dialog.open).toBe(false);
    http.expectNone((request) => request.url.includes('/history'));
  });

  it('preserves the selected period when retrying history', () => {
    const { fixture, page, dialog } = setup();
    doubleClick(page);
    expectHistory().flush('Failed', { status: 500, statusText: 'Error' });
    fixture.detectChanges();
    expect(dialog.textContent).toContain('Unable to load stock history');
    dialog.querySelector<HTMLButtonElement>('.error-message button')!.click();
    expectHistory().flush(history);
    fixture.detectChanges();
    expect(dialog.textContent).toContain('Delivery Results');
  });

  it('ignores stale responses when switching products or closing the dialog', () => {
    const { page, dialog } = setup();
    doubleClick(page);
    const first = expectHistory();
    doubleClick(page, { ...stock, productCode: 4000050 });
    const second = expectHistory(4000050);
    second.flush({ ...history, productCode: 4000050 });
    first.flush(history);
    expect(page.history?.productCode).toBe(4000050);
    doubleClick(page);
    const third = expectHistory();
    page.closeHistory();
    third.flush(history);
    expect(dialog.open).toBe(false);
    expect(page.history).toBeNull();
  });

  it('shows empty history and disables printing', () => {
    const { fixture, page, dialog } = setup();
    doubleClick(page);
    expectHistory().flush({ ...history, items: [] });
    fixture.detectChanges();
    expect(dialog.textContent).toContain('No stock history found');
    expect(
      dialog.querySelector<HTMLButtonElement>('.history-actions .search-button')!.disabled,
    ).toBe(true);
  });

  it('prints loaded history and closes with Quit', () => {
    const { fixture, page, dialog } = setup();
    const print = vi.spyOn(window, 'print').mockImplementation(() => {});
    try {
      doubleClick(page);
      expectHistory().flush(history);
      fixture.detectChanges();
      dialog.querySelector<HTMLButtonElement>('.history-actions .search-button')!.click();
      expect(print).toHaveBeenCalledOnce();
      dialog.querySelector<HTMLButtonElement>('.history-actions .retry-button')!.click();
      expect(dialog.open).toBe(false);
    } finally {
      print.mockRestore();
    }
  });

  it('loads all with pagination, caches the list and refreshes it explicitly', async () => {
    const { fixture, page } = setup();
    page.searchAll();
    http
      .expectOne(endpoint)
      .flush({ ...report, items: Array.from({ length: 51 }, (_, i) => ({ ...stock, id: i })) });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular)).componentInstance;
    expect(grid.rowData).toHaveLength(51);
    expect(grid.pagination).toBe(true);
    expect(grid.paginationPageSize).toBe(50);
    page.searchAll();
    http.expectNone(endpoint);
    page.refresh();
    http.expectOne(endpoint).flush(report);
    expect(page.stockItems).toEqual([stock]);
  });

  it('restores cached all results after an error and ignores outstanding product searches', () => {
    const { page } = setup();
    page.searchAll();
    http.expectOne(endpoint).flush(report);
    page.searchTerm = '400';
    page.searchByProductCode();
    const pending = http.expectOne(`${endpoint}/400`);
    page.searchAll();
    pending.flush({ ...report, items: [] });
    expect(page.stockItems).toEqual([stock]);
    page.searchTerm = 'invalid';
    page.searchByProductCode();
    expect(page.errorMessage).not.toBeNull();
    page.searchAll();
    expect(page.errorMessage).toBeNull();
    expect(page.loading).toBe(false);
    expect(page.stockItems).toEqual([stock]);
  });

  it('rejects invalid product codes without requesting data', () => {
    const { page } = setup();
    for (const code of ['104A', '12345678', '', '-1']) {
      page.searchTerm = code;
      page.searchByProductCode();
      expect(page.errorMessage).toContain('no more than 7 digits');
    }
    http.expectNone((request) => request.url.startsWith(endpoint));
  });

  if (kind === 'monthly') {
    it('selects each row month separately for the same product and ignores stale month responses', () => {
      const { page } = setup();
      doubleClick(page);
      const july = expectHistory();
      doubleClick(page, { ...stock, id: 2, reportDate: '2024-01-29' });
      const january = expectHistory(1040100, '2024-01-29');
      january.flush({ ...history, workingDayFrom: '2024-01-01', workingDayTo: '2024-01-31' });
      july.flush(history);
      expect(page.history?.workingDayFrom).toBe('2024-01-01');
    });

    it('does not guess a history month for records without a report date', () => {
      const { page, dialog } = setup();
      doubleClick(page, { ...stock, reportDate: null });
      expect(dialog.open).toBe(true);
      expect(page.historyError).toContain('no report date');
      expect(page.historyLoading).toBe(false);
      http.expectNone((request) => request.url.includes('/history'));
    });
  }
});

it('protects both new report routes with the logistics guard', () => {
  const children = routes.find((route) => route.path === '')!.children!;
  for (const kind of ['last', 'monthly']) {
    const route = children.find((route) => route.path === `logistic/${kind}-stock`)!;
    expect(route.data?.['stockReport']).toBe(kind);
    expect(route.canActivate).toContain(logisticsAccessGuard);
  }
});
