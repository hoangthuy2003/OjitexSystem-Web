import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { GoodsReceiptSearchPage } from './goods-receipt-search-page';
import { GoodsReceiptSearchResult } from './goods-receipt-search.models';

const mockResult: GoodsReceiptSearchResult = {
  items: [
    {
      key: 1,
      dateReceipt: '2026-11-09',
      receiveNo: 588354,
      customerCode: 518,
      customerName: 'DECATHLON OFFICE',
      productCode: 5180061,
      productName: 'CARTON B25 (410X280X300)',
      quantity: 79,
      productionSection: 'FFG1',
      note: 'Thanh',
      dateInput: '2026-11-09T08:30:00',
      userFirstName: 'Ms. Thanh',
      dateChanged: null,
      userChangedFirstName: null,
    },
  ],
};

describe('GoodsReceiptSearchPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(GoodsReceiptSearchPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('renders result rows after a successful search', async () => {
    const { fixture, http } = setup();
    fixture.componentInstance.dateReceipt = '11/09/2026';
    fixture.componentInstance.search();
    http.expectOne((req) => req.url === '/api/GoodsReceipt/search').flush(mockResult);
    fixture.detectChanges();
    const text = (fixture.nativeElement as HTMLElement).textContent ?? '';
    expect(text).toContain('DECATHLON OFFICE');
    expect(text).toContain('Ms. Thanh');
    http.verify();
  });

  it('shows the record count after searching', async () => {
    const { fixture, http } = setup();
    fixture.componentInstance.search();
    http.expectOne((req) => req.url === '/api/GoodsReceipt/search').flush(mockResult);
    fixture.detectChanges();
    const text = (fixture.nativeElement as HTMLElement).textContent ?? '';
    expect(text).toContain('1 record(s)');
    http.verify();
  });

  it('shows an error message on failure', () => {
    const { fixture, http } = setup();
    fixture.componentInstance.search();
    http
      .expectOne((req) => req.url === '/api/GoodsReceipt/search')
      .flush('error', { status: 500, statusText: 'Server Error' });
    expect(fixture.componentInstance.errorMessage).toBeTruthy();
    http.verify();
  });

  it('clears all state when clear() is called', () => {
    const { fixture, http } = setup();
    fixture.componentInstance.dateReceipt = '11/09/2026';
    fixture.componentInstance.receiveNo = '588354';
    fixture.componentInstance.search();
    http.expectOne((req) => req.url === '/api/GoodsReceipt/search').flush(mockResult);
    fixture.componentInstance.clear();
    expect(fixture.componentInstance.dateReceipt).toBe('');
    expect(fixture.componentInstance.receiveNo).toBe('');
    expect(fixture.componentInstance.items).toHaveLength(0);
    expect(fixture.componentInstance.errorMessage).toBeNull();
    http.verify();
  });

  it('pages records at pageSize intervals', () => {
    const { fixture, http } = setup();
    const manyItems = Array.from({ length: 25 }, (_, i) => ({
      ...mockResult.items[0],
      key: i + 1,
      receiveNo: i + 1,
    }));
    fixture.componentInstance.search();
    http.expectOne((req) => req.url === '/api/GoodsReceipt/search').flush({ items: manyItems });
    expect(fixture.componentInstance.pageCount).toBe(2);
    expect(fixture.componentInstance.pagedItems).toHaveLength(20);
    fixture.componentInstance.goToPage(1);
    expect(fixture.componentInstance.pagedItems).toHaveLength(5);
    http.verify();
  });
});
