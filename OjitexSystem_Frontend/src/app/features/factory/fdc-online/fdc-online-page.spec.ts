import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellApiModule, ColumnApiModule, ModuleRegistry, RowApiModule } from 'ag-grid-community';
import { FdcOnlinePage } from './fdc-online-page';
import { FdcOnlineItem, FdcOnlineReport } from './fdc-online.models';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule]);

const endpoint = '/api/factory/fdc-online';
const report: FdcOnlineReport = {
  reportDate: '2026-10-16',
  refreshedAt: '2026-10-16T09:04:00',
  items: [
    {
      shift: 1,
      lotNo: 5110,
      orderNo: 70876,
      customerCode: 115,
      customerName: 'NITORI',
      productCode: 1151773,
      productName: 'TOPPER TOP 170×198',
      planQty: 1027,
      inputQty: 1033,
      outputQty: 1025,
      surplusQty: 0,
      lossQty: null,
      shortestQty: 2,
      startTime: '08:30',
      finishTime: '08:37',
      drivingMinutes: 0.4,
      setMinutes: 6.8,
      stopMinutes: 0,
      restMinutes: 0,
    },
  ],
};

describe('FdcOnlinePage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FdcOnlinePage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage() {
    const fixture = TestBed.createComponent(FdcOnlinePage);
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
      .componentInstance as AgGridAngular<FdcOnlineItem>;
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
      '5110',
      '70876',
      '115',
      'NITORI',
      '1151773',
      'TOPPER TOP 170×198',
      '1,027',
      '1,033',
      '1,025',
      '0',
      '—',
      '2',
      '08:30',
      '08:37',
      '0.4',
      '6.8',
      '0',
      '0',
    ]);
    expect(fixture.nativeElement.textContent).toContain('10/16/2026 9:04 AM');
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(false);
    expect(fixture.nativeElement.querySelector('a.back-button').getAttribute('href')).toBe(
      '/department/factory',
    );
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
    expect(fixture.nativeElement.textContent).toContain('No FDC results for today.');
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
      .componentInstance as AgGridAngular<FdcOnlineItem>;
    expect(grid.api.paginationGetPageSize()).toBe(50);
    expect(grid.api.paginationGetTotalPages()).toBe(2);
    grid.api.applyColumnState({ state: [{ colId: 'orderNo', sort: 'desc' }] });
    expect(grid.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(grid.rowData).toEqual(items);
    grid.api.paginationGoToNextPage();
    expect(grid.api.paginationGetCurrentPage()).toBe(1);
  });
});
