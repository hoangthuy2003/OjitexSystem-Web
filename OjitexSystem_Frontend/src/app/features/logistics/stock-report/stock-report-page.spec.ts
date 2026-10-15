import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellDoubleClickedEvent } from 'ag-grid-community';
import { StockReportHistory, StockReportItem, StockReportKind } from './stock-report.models';
import { routes } from '../../../app.routes';
import { logisticsAccessGuard } from '../../../core/auth/guards/category-access.guard';
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

describe.each<StockReportKind>(['current', 'last', 'monthly'])('%s stock report', (kind) => {
  const endpoint = `/api/StockReport/${kind}`;
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
        (kind !== 'monthly'
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

  it('assigns equal flexible widths to all stock columns', () => {
    const component = TestBed.createComponent(StockReportPage).componentInstance;

    expect(component.defaultColDef.flex).toBe(1);
    expect(component.defaultColDef.minWidth).toBe(120);
    expect(component.defaultColDef.headerClass).toBe('center-header');
    expect(component.defaultColDef.cellStyle).toEqual({ textAlign: 'center' });
    expect(component.columnDefs.every((column) => column.width === undefined)).toBe(true);
  });

  it('copies a selected cell range as tab-separated values for spreadsheets', async () => {
    const fixture = TestBed.createComponent(StockReportPage);
    fixture.componentInstance.searchTerm = '1010010';
    fixture.componentInstance.searchByProductCode();
    fixture.detectChanges();
    http.expectOne(`${endpoint}/1010010`).flush(report);
    await fixture.whenStable();
    fixture.detectChanges();

    const container = fixture.nativeElement.querySelector('.table-scroll') as HTMLElement;
    const firstRow = document.createElement('div');
    firstRow.className = 'ag-row';
    firstRow.setAttribute('row-index', '100');
    const firstProductCell = document.createElement('div');
    firstProductCell.className = 'ag-cell';
    firstProductCell.setAttribute('col-id', 'productCode');
    firstProductCell.textContent = '1010060';
    const firstStockCell = document.createElement('div');
    firstStockCell.className = 'ag-cell';
    firstStockCell.setAttribute('col-id', 'openingStock');
    firstStockCell.textContent = '300';
    firstRow.append(firstProductCell, firstStockCell);

    const secondRow = document.createElement('div');
    secondRow.className = 'ag-row';
    secondRow.setAttribute('row-index', '101');
    const secondProductCell = document.createElement('div');
    secondProductCell.className = 'ag-cell';
    secondProductCell.setAttribute('col-id', 'productCode');
    secondProductCell.textContent = '1010061';
    const secondStockCell = document.createElement('div');
    secondStockCell.className = 'ag-cell';
    secondStockCell.setAttribute('col-id', 'openingStock');
    secondStockCell.textContent = '450';
    secondRow.append(secondProductCell, secondStockCell);
    container.append(firstRow, secondRow);

    const selection = window.getSelection();
    const range = document.createRange();
    range.setStart(firstProductCell.firstChild!, 0);
    range.setEnd(secondStockCell.firstChild!, secondStockCell.textContent!.length);
    selection!.removeAllRanges();
    selection!.addRange(range);

    const setData = vi.fn();
    const copyEvent = new Event('copy', { bubbles: true, cancelable: true });
    Object.defineProperty(copyEvent, 'clipboardData', { value: { setData } });
    container.dispatchEvent(copyEvent);

    expect(copyEvent.defaultPrevented).toBe(true);
    expect(setData).toHaveBeenCalledWith('text/plain', '1010060\t300\n1010061\t450');
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

it('uses the shared component and logistics guard for all three report routes', () => {
  const children = routes.find((route) => route.path === '')!.children!;
  for (const kind of ['current', 'last', 'monthly']) {
    const route = children.find((route) => route.path === `logistic/${kind}-stock`)!;
    expect(route.data?.['stockReport']).toBe(kind);
    expect(route.canActivate).toContain(logisticsAccessGuard);
  }
});
