import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { AgGridAngular } from 'ag-grid-angular';
import { CurrentStockPage } from './current-stock-page';

const stock = {
  cstProCd: 1010010,
  cstOpenStock: 11,
  cstWarehouseNg: 0,
  cstDisposal: 0,
  cstRepair: 0,
  cstProduction: 103,
  cstDelivery: 100,
  cstStock: 14,
};

describe('CurrentStockPage', () => {
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CurrentStockPage],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('waits for the user to choose a product search or search all', () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.detectChanges();

    expect((fixture.nativeElement as HTMLElement).textContent).toContain('Tìm tất cả');
    httpTesting.expectNone((request) => request.url.includes('/api/CurrentStock'));
  });

  it('searches product codes by prefix without downloading the full list', async () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.componentInstance.searchTerm = '1010010';
    fixture.componentInstance.searchByProductCode();
    fixture.detectChanges();

    const request = httpTesting.expectOne('/api/CurrentStock/search/1010010');
    expect(request.request.method).toBe('GET');
    request.flush([stock]);

    await fixture.whenStable();

    const element = fixture.nativeElement as HTMLElement;
    const grid = fixture.debugElement.query(By.directive(AgGridAngular));
    expect(grid.componentInstance.rowData).toEqual([stock]);
    expect(element.querySelector('.stock-grid')).not.toBeNull();
    expect(grid.componentInstance.pagination).toBe(false);
    expect(grid.componentInstance.enableCellTextSelection).toBe(true);
    expect(grid.componentInstance.ensureDomOrder).toBe(true);
    expect(element.textContent).toContain('Ctrl+C');
  });

  it('displays multiple products returned for a product code prefix', async () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.componentInstance.searchTerm = '100000';
    fixture.componentInstance.searchByProductCode();
    fixture.detectChanges();

    const request = httpTesting.expectOne('/api/CurrentStock/search/100000');
    request.flush([
      { ...stock, cstProCd: 1000001 },
      { ...stock, cstProCd: 1000002 },
      { ...stock, cstProCd: 1000003 },
    ]);

    await fixture.whenStable();
    fixture.detectChanges();

    const grid = fixture.debugElement.query(By.directive(AgGridAngular));
    expect(grid.componentInstance.rowData.map((item: typeof stock) => item.cstProCd)).toEqual([
      1000001,
      1000002,
      1000003,
    ]);
    expect(grid.componentInstance.pagination).toBe(true);
  });

  it('copies a selected cell range as tab-separated values for spreadsheets', async () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.componentInstance.searchTerm = '1010010';
    fixture.componentInstance.searchByProductCode();
    fixture.detectChanges();
    httpTesting.expectOne('/api/CurrentStock/search/1010010').flush([stock]);
    await fixture.whenStable();
    fixture.detectChanges();

    const container = fixture.nativeElement.querySelector('.table-scroll') as HTMLElement;
    const firstRow = document.createElement('div');
    firstRow.className = 'ag-row';
    firstRow.setAttribute('row-index', '100');
    const firstProductCell = document.createElement('div');
    firstProductCell.className = 'ag-cell';
    firstProductCell.setAttribute('col-id', 'cstProCd');
    firstProductCell.textContent = '1010060';
    const firstStockCell = document.createElement('div');
    firstStockCell.className = 'ag-cell';
    firstStockCell.setAttribute('col-id', 'cstOpenStock');
    firstStockCell.textContent = '300';
    firstRow.append(firstProductCell, firstStockCell);

    const secondRow = document.createElement('div');
    secondRow.className = 'ag-row';
    secondRow.setAttribute('row-index', '101');
    const secondProductCell = document.createElement('div');
    secondProductCell.className = 'ag-cell';
    secondProductCell.setAttribute('col-id', 'cstProCd');
    secondProductCell.textContent = '1010061';
    const secondStockCell = document.createElement('div');
    secondStockCell.className = 'ag-cell';
    secondStockCell.setAttribute('col-id', 'cstOpenStock');
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

  it('rejects invalid product codes without making an API request', () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.componentInstance.searchTerm = '10100A';
    fixture.componentInstance.searchByProductCode();
    fixture.detectChanges();

    expect((fixture.nativeElement as HTMLElement).textContent).toContain('tối đa 7 chữ số');
    httpTesting.expectNone((request) => request.url.includes('/api/CurrentStock/search/'));
  });

  it('loads the all endpoint once and enables AG Grid client-side pagination', async () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.detectChanges();
    fixture.componentInstance.searchAll();
    fixture.detectChanges();

    const getAll = httpTesting.expectOne('/api/CurrentStock');
    expect(getAll.request.params.keys()).toEqual([]);
    getAll.flush(
      Array.from({ length: 51 }, (_, index) => ({
        ...stock,
        cstProCd: stock.cstProCd + index,
      })),
    );

    await fixture.whenStable();
    fixture.detectChanges();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular));
    expect(grid.componentInstance.rowData).toHaveLength(51);
    expect(grid.componentInstance.pagination).toBe(true);
    expect(grid.componentInstance.paginationPageSize).toBe(50);
    expect(grid.componentInstance.paginationPageSizeSelector).toEqual([25, 50, 100]);
    httpTesting.expectNone('/api/CurrentStock');
  });

  it('uses the cached inventory when returning to search all and reloads only on refresh', async () => {
    const fixture = TestBed.createComponent(CurrentStockPage);
    fixture.componentInstance.searchAll();
    httpTesting.expectOne('/api/CurrentStock').flush([stock]);
    await fixture.whenStable();

    fixture.componentInstance.searchAll();
    httpTesting.expectNone('/api/CurrentStock');

    fixture.componentInstance.refresh();
    httpTesting.expectOne('/api/CurrentStock').flush([stock]);
    await fixture.whenStable();
    expect(fixture.componentInstance.stockItems).toEqual([stock]);
  });
});
