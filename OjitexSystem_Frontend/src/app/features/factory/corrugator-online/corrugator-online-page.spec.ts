import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellApiModule, ColumnApiModule, ModuleRegistry, RowApiModule } from 'ag-grid-community';
import { CorrugatorOnlinePage } from './corrugator-online-page';
import { CorrugatorOnlineItem, CorrugatorOnlineReport } from './corrugator-online.models';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule]);

const endpoint = '/api/factory/corrugator-online';
const report: CorrugatorOnlineReport = {
  reportDate: '2026-10-16',
  refreshedAt: '2026-10-16T09:04:00',
  items: [
    {
      planDate: '2026-10-16',
      orderNo: 10,
      targetCut: 315,
      qualityItem: 315,
      paperWidth: 1250,
      corruLength: 1250,
      sheetWidth: 200,
      corruUp: 6,
      flute: 'B',
      glue: 'JC 170',
      bm: 'IS 115',
      bl: 'JC 170',
      am: '',
      al: null,
      driveMinutes: 3.9,
      stopMinutes: 0.2,
      averageSpeed: 100,
      endTime: '08:00',
      corrugatedDate: '2026-10-16',
      dateInput: '2026-10-16T08:22:00',
    },
  ],
};

describe('CorrugatorOnlinePage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CorrugatorOnlinePage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage() {
    const fixture = TestBed.createComponent(CorrugatorOnlinePage);
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

  it('loads today on entry and displays all twenty columns in the result grid', async () => {
    const fixture = createPage();
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(true);
    http.expectOne(endpoint).flush(report);
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<CorrugatorOnlineItem>;
    const columns = grid.api.getColumns() ?? [];
    expect(columns).toHaveLength(20);
    expect(grid.rowData).toEqual(report.items);
    const rowNode = grid.api.getDisplayedRowAtIndex(0)!;
    const cells = columns.map((column) =>
      grid.api.getCellValue({ rowNode, colKey: column, useFormatter: true }),
    );
    expect(cells).toEqual([
      '10/16/2026',
      '10',
      '315',
      '315',
      '1,250',
      '1,250',
      '200',
      '6',
      'B',
      'JC 170',
      'IS 115',
      'JC 170',
      '—',
      '—',
      '3.9',
      '0.2',
      '100',
      '08:00',
      '10/16/2026',
      '10/16/2026 8:22',
    ]);
    expect(fixture.nativeElement.textContent).toContain('10/16/2026 9:04 AM');
    expect(button(fixture.nativeElement, 'Refresh').disabled).toBe(false);
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
    expect(fixture.nativeElement.textContent).toContain('No Corrugator results for today.');
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
      .componentInstance as AgGridAngular<CorrugatorOnlineItem>;
    expect(grid.api.paginationGetPageSize()).toBe(50);
    expect(grid.api.paginationGetTotalPages()).toBe(2);
    grid.api.applyColumnState({ state: [{ colId: 'orderNo', sort: 'desc' }] });
    expect(grid.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(grid.rowData).toEqual(items);
    grid.api.paginationGoToNextPage();
    expect(grid.api.paginationGetCurrentPage()).toBe(1);
  });
});
