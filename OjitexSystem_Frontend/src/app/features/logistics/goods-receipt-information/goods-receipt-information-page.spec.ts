import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { GoodsReceiptInformationReport } from './goods-receipt-information.models';
import { GoodsReceiptInformationPage } from './goods-receipt-information-page';

const mockReport: GoodsReceiptInformationReport = {
  items: [
    {
      key: 1,
      dateReceipt: '2026-11-10',
      receiveNo: 581322,
      customerCode: 501,
      customerName: 'B Braun VN',
      productCode: 5010037,
      productName: 'Carton Box',
      quantity: 480,
      productionSection: 'ST',
      note: '09/11/2026',
      dateInput: '2026-11-10T07:32:00',
      userFirstName: 'Ms. Thanh',
      dateChanged: null,
      userChangedFirstName: null,
    },
  ],
};

describe('GoodsReceiptInformationPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(GoodsReceiptInformationPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests goods receipt information with trimmed filters', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      report: () => GoodsReceiptInformationReport | null;
    };
    page.changeFilter('dateReceipt', ' 11/10/2026 ');
    page.changeFilter('receiveNo', ' 581322 ');
    page.changeFilter('productCode', ' 5010037 ');
    page.search();

    const request = http.expectOne((req) => req.url === '/api/GoodsReceipt/information');
    expect(request.request.params.get('dateReceipt')).toBe('11/10/2026');
    expect(request.request.params.get('receiveNo')).toBe('581322');
    expect(request.request.params.get('productCode')).toBe('5010037');

    request.flush(mockReport);
    expect(page.report()).toEqual(mockReport);
    http.verify();
  });

  it('rejects invalid dateReceipt or productCode', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      error: () => string | null;
    };

    page.changeFilter('dateReceipt', '99/99/9999');
    page.search();
    expect(page.error()).toContain('valid date');

    page.changeFilter('dateReceipt', '11/10/2026');
    page.changeFilter('productCode', 'abc');
    page.search();
    expect(page.error()).toContain('Product code');

    http.expectNone('/api/GoodsReceipt/information');
    http.verify();
  });

  it('clears results and filters on clear()', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => GoodsReceiptInformationReport | null;
      filters: () => { receiveNo: string };
    };

    page.changeFilter('receiveNo', '581322');
    page.search();
    http.expectOne((req) => req.url === '/api/GoodsReceipt/information').flush(mockReport);
    expect(page.report()).toEqual(mockReport);

    page.clear();
    expect(page.report()).toBeNull();
    expect(page.filters().receiveNo).toBe('');
    http.verify();
  });
});
