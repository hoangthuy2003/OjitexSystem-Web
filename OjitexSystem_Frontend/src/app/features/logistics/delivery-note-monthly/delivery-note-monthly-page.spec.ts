import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryMonthlyReport } from './delivery-note-monthly.models';
import { DeliveryNoteMonthlyPage } from './delivery-note-monthly-page';

const mockReport: DeliveryMonthlyReport = {
  items: [
    {
      key: 1,
      status: 'Done',
      lineNo: 1,
      deliverySlip: '71343',
      receiveNo: 0,
      supplierCode: null,
      customerCode: 134,
      customerName: 'SONIC APPLIANCES',
      productCode: 1340040,
      productName: '016-8DV00 CORNER P',
      quantity: 2800,
      deliveryDate: '2026-11-09',
      customerPo: '5700318761-110',
      remark: '6H',
      otherNote: '3 PL 1400*1600',
      truckNumber: '29E61740',
      carType: '8',
      transportCompany: 'NKV',
      dateInput: '2026-11-07T16:06:00',
      userId: 'vinh',
      userFirstName: 'Mr. Vinh',
      state: 'Updated',
    },
  ],
};

describe('DeliveryNoteMonthlyPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryNoteMonthlyPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests monthly delivery information with trimmed filters', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      report: () => DeliveryMonthlyReport | null;
    };
    page.changeFilter('deliverySlip', ' 71343 ');
    page.changeFilter('customerCode', ' 134 ');
    page.changeFilter('productCode', ' 1340040 ');
    page.changeFilter('fromDate', ' 11/01/2026 ');
    page.changeFilter('toDate', ' 11/09/2026 ');
    page.search();

    const request = http.expectOne((req) => req.url === '/api/DeliveryNote/monthly');
    expect(request.request.params.get('deliverySlip')).toBe('71343');
    expect(request.request.params.get('customerCode')).toBe('134');
    expect(request.request.params.get('productCode')).toBe('1340040');
    expect(request.request.params.get('fromDate')).toBe('11/01/2026');
    expect(request.request.params.get('toDate')).toBe('11/09/2026');

    request.flush(mockReport);
    expect(page.report()).toEqual(mockReport);
    http.verify();
  });

  it('rejects invalid fromDate, toDate or date ranges', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      error: () => string | null;
    };

    page.changeFilter('fromDate', 'invalid');
    page.search();
    expect(page.error()).toContain('valid from date');

    page.changeFilter('fromDate', '11/09/2026');
    page.changeFilter('toDate', '11/01/2026');
    page.search();
    expect(page.error()).toContain('earlier than or equal to');

    http.expectNone('/api/DeliveryNote/monthly');
    http.verify();
  });

  it('clears results and resets to default dates on clear()', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => DeliveryMonthlyReport | null;
      filters: () => { deliverySlip: string };
    };

    page.changeFilter('deliverySlip', '71343');
    page.search();
    http.expectOne((req) => req.url === '/api/DeliveryNote/monthly').flush(mockReport);
    expect(page.report()).toEqual(mockReport);

    page.clear();
    expect(page.report()).toBeNull();
    expect(page.filters().deliverySlip).toBe('');
    http.verify();
  });
});
