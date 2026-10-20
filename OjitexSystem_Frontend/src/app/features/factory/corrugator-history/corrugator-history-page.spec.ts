import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AgGridAngular } from 'ag-grid-angular';
import { CellApiModule, ColumnApiModule, ModuleRegistry, RowApiModule } from 'ag-grid-community';
import { CorrugatorReport, CorrugatorReportItem } from '../corrugator-report.models';
import { CorrugatorHistoryPage } from './corrugator-history-page';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule]);

const endpoint = '/api/factory/corrugator-history';
const report: CorrugatorReport = {
  reportDate: '2026-10-19',
  refreshedAt: '2026-10-20T09:00:00',
  items: [
    {
      planDate: '2026-10-19',
      orderNo: 10,
      targetCut: 40,
      qualityItem: 40,
      paperWidth: 2150,
      corruLength: 1155,
      sheetWidth: 1057,
      corruUp: 2,
      flute: 'B',
      glue: 'DHC 125',
      bm: 'S  125',
      bl: 'DHC 125',
      am: '',
      al: null,
      driveMinutes: 0.7,
      stopMinutes: 0,
      averageSpeed: 66,
      endTime: '07:58',
      corrugatedDate: '2026-10-19',
      dateInput: '2026-10-19T08:22:00',
    },
  ],
};

describe('CorrugatorHistoryPage', () => {
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CorrugatorHistoryPage],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  function createPage() {
    const fixture = TestBed.createComponent(CorrugatorHistoryPage);
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

  function enterDate(element: HTMLElement, date: string): void {
    const input = element.querySelector('input')!;
    input.value = date;
    input.dispatchEvent(new Event('input'));
  }

  function expectSearch(date = '10/19/2026') {
    return http.expectOne(
      (request) => request.url === endpoint && request.params.get('date') === date,
    );
  }

  it('waits for a date and provides the expected input format and Factory back link', () => {
    const fixture = createPage();
    http.expectNone((request) => request.url === endpoint);
    expect(fixture.nativeElement.querySelector('input').placeholder).toBe('mm/dd/yyyy');
    expect(fixture.nativeElement.textContent).toContain('Enter a Corrugator date');
    expect(fixture.nativeElement.querySelector('.back-button').getAttribute('href')).toBe(
      '/department/factory',
    );
  });

  it.each([
    '',
    '19/10/2026',
    '2026-10-19',
    '1/10/2026',
    '04/31/2026',
    '02/29/2026',
    '00/10/2026',
    '10/00/2026',
    '10/19/0000',
  ])('rejects invalid date %s without querying the server', (date) => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, date);
    button(fixture.nativeElement, 'Search').click();
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain(
      'mm/dd/yyyy',
    );
    expect(fixture.nativeElement.querySelector('input').getAttribute('aria-invalid')).toBe('true');
    http.expectNone((request) => request.url === endpoint);
  });

  it.each(['03/04/2026', '02/29/2024', '02/29/2000'])(
    'sends the valid month/day/year date %s unchanged',
    (date) => {
      const fixture = createPage();
      enterDate(fixture.nativeElement, date);
      button(fixture.nativeElement, 'Search').click();
      expectSearch(date).flush({ ...report, items: [] });
    },
  );

  it('shows the twenty report columns, formatting, clipboard and pagination consistently with Online', async () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    fixture.detectChanges();
    expect(button(fixture.nativeElement, 'Search').disabled).toBe(true);
    expectSearch().flush(report);
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<CorrugatorReportItem>;
    const columns = grid.api.getColumns() ?? [];
    expect(columns).toHaveLength(20);
    expect(grid.rowData).toEqual(report.items);
    const rowNode = grid.api.getDisplayedRowAtIndex(0)!;
    expect(
      columns.map((column) =>
        grid.api.getCellValue({ rowNode, colKey: column, useFormatter: true }),
      ),
    ).toEqual([
      '10/19/2026',
      '10',
      '40',
      '40',
      '2,150',
      '1,155',
      '1,057',
      '2',
      'B',
      'DHC 125',
      'S  125',
      'DHC 125',
      '—',
      '—',
      '0.7',
      '0',
      '66',
      '07:58',
      '10/19/2026',
      '10/19/2026 8:22',
    ]);
    expect(grid.paginationPageSizeSelector).toEqual([25, 50, 100]);
    expect(
      fixture.nativeElement.querySelector('ag-grid-angular').hasAttribute('reportClipboard'),
    ).toBe(true);
    expect(button(fixture.nativeElement, 'Search').disabled).toBe(false);
  });

  it('uses a fresh request when searching again and distinguishes empty results', () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    expectSearch().flush(report);
    fixture.detectChanges();
    button(fixture.nativeElement, 'Search').click();
    expectSearch().flush({ ...report, items: [] });
    fixture.detectChanges();
    expect(fixture.nativeElement.textContent).toContain('No Corrugator results for 10/19/2026.');
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
  });

  it('cancels the previous search when the input changes and clears the displayed snapshot', () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    const pending = expectSearch();
    enterDate(fixture.nativeElement, '10/18/2026');
    fixture.detectChanges();
    expect(pending.cancelled).toBe(true);
    expect(button(fixture.nativeElement, 'Search').disabled).toBe(false);
    button(fixture.nativeElement, 'Search').click();
    expectSearch('10/18/2026').flush({ ...report, reportDate: '2026-10-18' });
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).not.toBeNull();
    enterDate(fixture.nativeElement, '10/17/2026');
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
  });

  it('Clear cancels requests, resets the date and lets the user search again', () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    const pending = expectSearch();
    fixture.detectChanges();
    button(fixture.nativeElement, 'Clear').click();
    fixture.detectChanges();
    expect(pending.cancelled).toBe(true);
    expect(fixture.nativeElement.querySelector('input').value).toBe('');
    expect(fixture.nativeElement.textContent).toContain('Enter a Corrugator date');
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    expectSearch().flush(report);
    fixture.detectChanges();
    button(fixture.nativeElement, 'Clear').click();
    fixture.detectChanges();
    expect(fixture.debugElement.query(By.directive(AgGridAngular))).toBeNull();
    http.expectNone((request) => request.url === endpoint);
  });

  it.each([403, 500])('shows an API error (%s) and allows another search', (status) => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    expectSearch().flush({}, { status, statusText: 'Error' });
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]').textContent).toContain(
      status === 403 ? 'does not have access' : 'Unable to load Corrugator History',
    );
    button(fixture.nativeElement, 'Search').click();
    expectSearch().flush(report);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('[role="alert"]')).toBeNull();
  });

  it('cancels a pending search when the page is destroyed', () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    const pending = expectSearch();
    fixture.destroy();
    expect(pending.cancelled).toBe(true);
  });

  it('paginates and sorts numeric values while preserving duplicate source rows', async () => {
    const fixture = createPage();
    enterDate(fixture.nativeElement, '10/19/2026');
    button(fixture.nativeElement, 'Search').click();
    const items = Array.from({ length: 51 }, () => ({ ...report.items[0] }));
    items[50] = { ...report.items[0], orderNo: 20 };
    expectSearch().flush({ ...report, items });
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular))
      .componentInstance as AgGridAngular<CorrugatorReportItem>;
    expect(grid.api.paginationGetPageSize()).toBe(50);
    expect(grid.api.paginationGetTotalPages()).toBe(2);
    grid.api.applyColumnState({ state: [{ colId: 'orderNo', sort: 'desc' }] });
    expect(grid.api.getDisplayedRowAtIndex(0)?.data).toEqual(items[50]);
    expect(grid.rowData).toEqual(items);
    grid.api.paginationGoToNextPage();
    expect(grid.api.paginationGetCurrentPage()).toBe(1);
  });
});
