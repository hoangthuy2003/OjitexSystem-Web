import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import {
  CellApiModule,
  ColumnApiModule,
  ModuleRegistry,
  RowApiModule,
  ScrollApiModule,
} from 'ag-grid-community';
import { EvolOnlinePage } from './evol-online-page';
import { EvolOnlineItem, EvolOnlineReport } from './evol-online.models';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule, ScrollApiModule]);

const endpoint = '/api/factory/evol-online';
const report: EvolOnlineReport = {
  reportDate: '2026-10-16',
  refreshedAt: '2026-10-16T09:04:00',
  items: [
    {
      shift: 1,
      lotNo: 5350,
      orderNo: 69836,
      customerCode: 506,
      customerName: 'DENKO',
      productCode: 5060131,
      productName: 'Nitto - Thùng carton',
      planQty: 3000,
      inputQty: 1,
      outputQty: 1,
      surplusQty: 0,
      lossQty: null,
      shortestQty: 2999,
      startTime: '08:58',
      finishTime: '13:02',
      drivingMinutes: 0,
      setMinutes: 1.9,
      stopMinutes: 0,
      restMinutes: 221.8,
    },
  ],
};

describe('EvolOnlinePage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [EvolOnlinePage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage() {
    const fixture = TestBed.createComponent(EvolOnlinePage);
    fixture.detectChanges();
    return fixture;
  }

  function button(element: HTMLElement, label: string): HTMLButtonElement {
    const result = Array.from(element.querySelectorAll('button')).find(
      (candidate) => candidate.textContent?.trim() === label,
    );
    if (!result) throw new Error(`Missing button: ${label}`);
    return result;
  }

  it('loads today on entry and displays all nineteen columns in the result grid', async () => {
    const fixture = createPage();
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(true);
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<EvolOnlineItem>;
    const columns = grid.api.getColumns() ?? [];
    expect(columns.map((column) => column.getColDef().headerName)).toEqual([
      'Shift',
      'Order No.',
      'Receive No.',
      'Customer Code',
      'Customer Name',
      'Product Code',
      'Product Name',
      'Plan Qty',
      'Input Qty',
      'Output Qty',
      'Surplus Qty',
      'Loss Qty',
      'Shortest Qty',
      'Start Time',
      'Finish Time',
      'Driving Time (Minutes)',
      'Set Time (Minutes)',
      'Stop Time (Minutes)',
      'Rest Time (Minutes)',
    ]);
    expect(grid.rowData).toEqual(report.items);
    const rowNode = grid.api.getDisplayedRowAtIndex(0)!;
    const cells = columns.map((column) =>
      grid.api.getCellValue({ rowNode, colKey: column, useFormatter: true }),
    );
    expect(cells).toEqual([
      '1',
      '5350',
      '69836',
      '506',
      'DENKO',
      '5060131',
      'Nitto - Thùng carton',
      '3,000',
      '1',
      '1',
      '0',
      '—',
      '2,999',
      '08:58',
      '13:02',
      '0',
      '1.9',
      '0',
      '221.8',
    ]);
    expect(fixture.nativeElement.textContent).toContain('10/16/2026 9:04 AM');
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(false);
    expect(fixture.nativeElement.querySelector('a.back-button').getAttribute('href')).toBe(
      '/department/factory',
    );
  });

  it('renders positive and zero shortfalls with normal cell styling', async () => {
    const fixture = createPage();
    http.expectOne(endpoint).flush({
      ...report,
      items: [report.items[0], { ...report.items[0], shortestQty: 0 }],
    });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<EvolOnlineItem>;
    grid.api.ensureColumnVisible('shortestQty');
    const cell = (row: number): HTMLElement | null =>
      fixture.nativeElement.querySelector(`[row-index="${row}"] [col-id="shortestQty"]`);
    await vi.waitFor(() => {
      expect(cell(0)?.style.backgroundColor).toBe('');
      expect(cell(0)?.style.color).toBe('');
      expect(cell(0)?.textContent).toBe('2,999');
      expect(cell(1)?.style.backgroundColor).toBe('');
      expect(cell(1)?.style.color).toBe('');
      expect(cell(1)?.textContent).toBe('0');
    });
  });

  it('refreshes from the API instead of reusing the previous snapshot', () => {
    const fixture = createPage();
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    button(fixture.nativeElement, 'Refresh').click();
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain("Loading today's results");
    http.expectOne(endpoint).flush({ ...report, items: [] });
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('No EVOL results for today.');
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
  });

  it('clear cancels a pending request and lets the user refresh again', () => {
    const fixture = createPage();
    const pending = http.expectOne(endpoint);
    button(fixture.nativeElement, 'Clear').click();
    fixture.detectChanges();
    expect(pending.cancelled).toBe(true);
    expect(fixture.nativeElement.textContent).toContain('Results cleared.');
    button(fixture.nativeElement, 'Refresh').click();
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    expect(
      fixture.debugElement.query(By.directive(AgGridAngular)).componentInstance.rowData,
    ).toEqual(report.items);
  });

  it('clear removes already loaded results without calling the server', () => {
    const fixture = createPage();
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    button(fixture.nativeElement, 'Clear').click();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
    expect(fixture.nativeElement.textContent).toContain('Results cleared.');
    http.expectNone(endpoint);
  });

  it('shows access errors and allows a subsequent refresh', () => {
    const fixture = createPage();
    http.expectOne(endpoint).flush({}, { status: 403, statusText: 'Forbidden' });
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain(
      'does not have access',
    );
    button(fixture.nativeElement, 'Refresh').click();
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]')).toBeNull();
    expect(
      fixture.debugElement.query(By.directive(AgGridAngular)).componentInstance.rowData,
    ).toEqual(report.items);
  });

  it('cancels requests when the page is destroyed', () => {
    const fixture = createPage();
    const pending = http.expectOne(endpoint);
    fixture.destroy();
    expect(pending.cancelled).toBe(true);
  });

  it('paginates all results and sorts numeric columns without changing row values', async () => {
    const fixture = createPage();
    const items = Array.from({ length: 51 }, (_, index) => ({
      ...report.items[0],
      orderNo: index + 1,
    }));
    http.expectOne(endpoint).flush({ ...report, items });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<EvolOnlineItem>;
    expect(grid.api.paginationGetPageSize()).toBe(50);
    expect(grid.api.paginationGetTotalPages()).toBe(2);
    grid.api.applyColumnState({ state: [{ colId: 'orderNo', sort: 'desc' }] });
    expect(grid.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(grid.rowData).toEqual(items);
    grid.api.paginationGoToNextPage();
    expect(grid.api.paginationGetCurrentPage()).toBe(1);
  });
});
