import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellApiModule, ColumnApiModule, ModuleRegistry, RowApiModule } from 'ag-grid-community';
import { ConvertingHistoryPage } from './converting-history-page';
import {
  ConvertingHistoryFilters,
  ConvertingHistoryItem,
  ConvertingHistoryReport,
} from './converting-history.models';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule]);

const endpoint = '/api/factory/converting-history';
const report: ConvertingHistoryReport = {
  reportDate: null,
  refreshedAt: '2026-10-20T09:00:00',
  items: [
    {
      date: '2026-10-19',
      shift: 2,
      machineName: 'FDC',
      lotNo: 1280,
      orderNo: 75142,
      customerCode: 115,
      customerName: 'NITORI',
      productCode: 1153161,
      productName: 'TOPPER',
      planQty: 30,
      inputQty: 36,
      outputQty: 34,
      surplusQty: 0,
      lossQty: 0,
      shortestQty: 0,
      startTime: '00:28',
      finishTime: '00:45',
      drivingMinutes: 0.5,
      setMinutes: 16.1,
      stopMinutes: 0,
      restMinutes: 0,
    },
  ],
};

type PageFixture = ComponentFixture<ConvertingHistoryPage>;

describe('ConvertingHistoryPage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ConvertingHistoryPage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage(): PageFixture {
    const fixture = TestBed.createComponent(ConvertingHistoryPage);
    fixture.detectChanges();
    return fixture;
  }

  function setFilter(
    fixture: PageFixture,
    field: keyof ConvertingHistoryFilters,
    value: string,
  ): void {
    const input = fixture.nativeElement.querySelector(`input[name="${field}"]`) as HTMLInputElement;
    input.value = value;
    input.dispatchEvent(new Event('input'));
    fixture.detectChanges();
  }

  function submit(fixture: PageFixture): void {
    fixture.nativeElement
      .querySelector('form')
      .dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    fixture.detectChanges();
  }

  function clear(fixture: PageFixture): void {
    fixture.nativeElement.querySelector('.clear-button').click();
    fixture.detectChanges();
  }

  function grid(fixture: PageFixture): AgGridAngular<ConvertingHistoryItem> {
    return fixture.debugElement.query(By.directive(AgGridAngular)).componentInstance;
  }

  it('waits for an explicit search and keeps the Search label with blank criteria', () => {
    const fixture = createPage();
    http.expectNone((request) => request.url === endpoint);
    expect(fixture.nativeElement.querySelector('[type="submit"]').textContent.trim()).toBe(
      'Search',
    );
    expect(fixture.nativeElement.querySelector('input[name="date"]').placeholder).toBe(
      'mm/dd/yyyy',
    );
    expect(fixture.nativeElement.querySelector('.back-button').getAttribute('href')).toBe(
      '/department/factory',
    );
  });

  it.each([
    [false, false, false],
    [true, false, false],
    [false, true, false],
    [false, false, true],
    [true, true, false],
    [true, false, true],
    [false, true, true],
    [true, true, true],
  ])('sends only selected criteria (date=%s, process=%s, product=%s)', (date, process, product) => {
    const fixture = createPage();
    if (date) setFilter(fixture, 'date', '10/19/2026');
    if (process) setFilter(fixture, 'process', 'fdc');
    if (product) setFilter(fixture, 'productCode', '1153');
    expect(fixture.nativeElement.querySelector('[type="submit"]').textContent.trim()).toBe(
      'Search',
    );
    submit(fixture);
    const request = http.expectOne((request) => request.url === endpoint);
    expect(request.request.params.get('date')).toBe(date ? '10/19/2026' : null);
    expect(request.request.params.get('process')).toBe(process ? 'fdc' : null);
    expect(request.request.params.get('productCode')).toBe(product ? '1153' : null);
    request.flush({ ...report, items: [] });
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('No matching Converting history found.');
  });

  it('trims criteria and accepts product code zero', () => {
    const fixture = createPage();
    setFilter(fixture, 'date', ' 10/19/2026 ');
    setFilter(fixture, 'process', ' fdc ');
    setFilter(fixture, 'productCode', ' 0 ');
    submit(fixture);
    const request = http.expectOne((request) => request.url === endpoint);
    expect(request.request.params.get('date')).toBe('10/19/2026');
    expect(request.request.params.get('process')).toBe('fdc');
    expect(request.request.params.get('productCode')).toBe('0');
    request.flush(report);
  });

  it.each([
    ['date', '19/10/2026'],
    ['date', '02/29/2026'],
    ['date', '04/31/2026'],
    ['productCode', '-1'],
    ['productCode', '1.5'],
    ['productCode', '11%'],
    ['productCode', '12345678'],
    ['process', 'TOOLONG'],
  ] as const)('rejects invalid %s=%s before sending a request', (field, value) => {
    const fixture = createPage();
    setFilter(fixture, field, value);
    submit(fixture);
    expect(fixture.nativeElement.querySelector('[role="alert"]')).not.toBeNull();
    expect(
      fixture.nativeElement.querySelector(`input[name="${field}"]`).getAttribute('aria-invalid'),
    ).toBe('true');
    http.expectNone((request) => request.url === endpoint);
  });

  it('renders the 21 SQL columns with report formatting, duplicate rows and clipboard support', async () => {
    const fixture = createPage();
    submit(fixture);
    const items = [report.items[0], report.items[0]];
    http.expectOne(endpoint).flush({ ...report, items });
    fixture.detectChanges();
    await fixture.whenStable();
    const table = grid(fixture);
    const columns = table.api.getColumns() ?? [];
    expect(columns).toHaveLength(21);
    expect(table.rowData).toEqual(items);
    const rowNode = table.api.getDisplayedRowAtIndex(0)!;
    expect(
      columns.map((column) =>
        table.api.getCellValue({ rowNode, colKey: column, useFormatter: true }),
      ),
    ).toEqual([
      '10/19/2026',
      '2',
      'FDC',
      '1280',
      '75142',
      '115',
      'NITORI',
      '1153161',
      'TOPPER',
      '30',
      '36',
      '34',
      '0',
      '0',
      '0',
      '00:28',
      '00:45',
      '0.5',
      '16.1',
      '0',
      '0',
    ]);
    expect(
      fixture.nativeElement.querySelector('ag-grid-angular').hasAttribute('reportClipboard'),
    ).toBe(true);
    expect(table.paginationPageSizeSelector).toEqual([25, 50, 100]);
  });

  it.each(['date', 'process', 'productCode'] as const)(
    'cancels the previous request when %s changes and searches the new criteria',
    (field) => {
      const fixture = createPage();
      submit(fixture);
      const previous = http.expectOne(endpoint);
      setFilter(
        fixture,
        field,
        field === 'date' ? '10/19/2026' : field === 'process' ? 'FG' : '115',
      );
      expect(previous.cancelled).toBe(true);
      expect(fixture.nativeElement.querySelector('[type="submit"]').disabled).toBe(false);
      submit(fixture);
      http.expectOne((request) => request.url === endpoint).flush(report);
      fixture.detectChanges();
      expect(grid(fixture).rowData).toEqual(report.items);
      setFilter(fixture, 'process', 'FAP');
      expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
    },
  );

  it('Clear cancels pending searches and resets all fields and displayed results', () => {
    const fixture = createPage();
    setFilter(fixture, 'date', '10/19/2026');
    setFilter(fixture, 'process', 'FDC');
    setFilter(fixture, 'productCode', '115');
    submit(fixture);
    const pending = http.expectOne((request) => request.url === endpoint);
    clear(fixture);
    expect(pending.cancelled).toBe(true);
    const inputs = Array.from(
      fixture.nativeElement.querySelectorAll('input'),
    ) as HTMLInputElement[];
    expect(inputs.every((input) => input.value === '')).toBe(true);
    expect(fixture.nativeElement.querySelector('[type="submit"]').textContent.trim()).toBe(
      'Search',
    );
    submit(fixture);
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    clear(fixture);
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
    http.expectNone(endpoint);
  });

  it('prevents duplicate submits but loads a fresh snapshot on a later search', () => {
    const fixture = createPage();
    submit(fixture);
    submit(fixture);
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    submit(fixture);
    http.expectOne(endpoint).flush({ ...report, items: [] });
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('No matching Converting history found.');
  });

  it.each([403, 500])('shows the API error (%s) and allows another search', (status) => {
    const fixture = createPage();
    submit(fixture);
    http.expectOne(endpoint).flush({}, { status, statusText: 'Error' });
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain(
      status === 403 ? 'does not have access' : 'Unable to load Converting History',
    );
    submit(fixture);
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]')).toBeNull();
  });

  it('cancels pending requests when the page is destroyed', () => {
    const fixture = createPage();
    submit(fixture);
    const request = http.expectOne(endpoint);
    fixture.destroy();
    expect(request.cancelled).toBe(true);
  });

  it('paginates and sorts numeric quantities without changing source values', async () => {
    const fixture = createPage();
    submit(fixture);
    const items = Array.from({ length: 51 }, (_, index) => ({
      ...report.items[0],
      outputQty: index,
    }));
    http.expectOne(endpoint).flush({ ...report, items });
    fixture.detectChanges();
    await fixture.whenStable();
    const table = grid(fixture);
    expect(table.api.paginationGetPageSize()).toBe(50);
    expect(table.api.paginationGetTotalPages()).toBe(2);
    table.api.applyColumnState({ state: [{ colId: 'outputQty', sort: 'desc' }] });
    expect(table.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(table.rowData).toEqual(items);
    table.api.paginationGoToNextPage();
    expect(table.api.paginationGetCurrentPage()).toBe(1);
  });
});
