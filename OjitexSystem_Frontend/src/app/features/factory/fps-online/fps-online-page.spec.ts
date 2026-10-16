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
import { FpsOnlinePage } from './fps-online-page';
import { FpsOnlineItem, FpsOnlineReport } from './fps-online.models';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule, ScrollApiModule]);

const endpoint = '/api/factory/fps-online';
const report: FpsOnlineReport = {
  reportDate: '2026-10-16',
  refreshedAt: '2026-10-16T14:00:00',
  items: [
    {
      shift: 1,
      lotNo: 5040,
      orderNo: 70163,
      customerCode: 701,
      customerName: 'THIEN THANH',
      productCode: 7011680,
      productName: 'S115*3/C175',
      planQty: 320,
      inputQty: 3,
      outputQty: 3,
      surplusQty: 0,
      lossQty: 0,
      shortestQty: 317,
      startTime: '09:22',
      finishTime: '12:30',
      drivingMinutes: 0,
      setMinutes: 2,
      stopMinutes: 1.1,
      restMinutes: 184.5,
    },
  ],
};

describe('FpsOnlinePage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FpsOnlinePage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage() {
    const fixture = TestBed.createComponent(FpsOnlinePage);
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
      .componentInstance as AgGridAngular<FpsOnlineItem>;
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
      '5040',
      '70163',
      '701',
      'THIEN THANH',
      '7011680',
      'S115*3/C175',
      '320',
      '3',
      '3',
      '0',
      '0',
      '317',
      '09:22',
      '12:30',
      '0',
      '2',
      '1.1',
      '184.5',
    ]);
    expect(fixture.nativeElement.textContent).toContain('10/16/2026 2:00 PM');
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(false);
    expect(fixture.nativeElement.querySelector('a.back-button').getAttribute('href')).toBe(
      '/department/factory',
    );
  });

  it('renders positive and zero shortfalls with normal cell styling', async () => {
    const fixture = createPage();
    http.expectOne(endpoint).flush({
      ...report,
      items: [report.items[0], { ...report.items[0], outputQty: 320, shortestQty: 0 }],
    });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<FpsOnlineItem>;
    grid.api.ensureColumnVisible('shortestQty');
    const cell = (row: number): HTMLElement | null =>
      fixture.nativeElement.querySelector(`[row-index="${row}"] [col-id="shortestQty"]`);
    await vi.waitFor(() => {
      expect(cell(0)?.style.backgroundColor).toBe('');
      expect(cell(0)?.style.color).toBe('');
      expect(cell(0)?.textContent).toBe('317');
      expect(cell(1)?.style.backgroundColor).toBe('');
      expect(cell(1)?.style.color).toBe('');
      expect(cell(1)?.textContent).toBe('0');
    });
  });

  it('keeps repeated machine results as separate rows', async () => {
    const fixture = createPage();
    const items = [
      report.items[0],
      {
        ...report.items[0],
        inputQty: 326,
        outputQty: 240,
        shortestQty: 80,
        startTime: '12:30',
        finishTime: '13:04',
        drivingMinutes: 2.4,
        setMinutes: 5.7,
        stopMinutes: 26.4,
        restMinutes: 0,
      },
      report.items[0],
    ];
    http.expectOne(endpoint).flush({ ...report, items });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<FpsOnlineItem>;
    expect(grid.api.getDisplayedRowCount()).toBe(3);
    expect([0, 1, 2].map((index) => grid.api.getDisplayedRowAtIndex(index)?.data)).toEqual(items);
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
    expect(fixture.nativeElement.textContent).toContain('No FPS results for today.');
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
      .componentInstance as AgGridAngular<FpsOnlineItem>;
    expect(grid.api.paginationGetPageSize()).toBe(50);
    expect(grid.api.paginationGetTotalPages()).toBe(2);
    grid.api.applyColumnState({ state: [{ colId: 'orderNo', sort: 'desc' }] });
    expect(grid.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(grid.rowData).toEqual(items);
    grid.api.paginationGoToNextPage();
    expect(grid.api.paginationGetCurrentPage()).toBe(1);
  });
});
