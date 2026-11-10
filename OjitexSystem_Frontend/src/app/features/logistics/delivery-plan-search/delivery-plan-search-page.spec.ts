import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryPlanSearchResult } from './delivery-plan-search.models';
import { DeliveryPlanSearchPage } from './delivery-plan-search-page';

const mockResult: DeliveryPlanSearchResult = {
  items: [
    {
      acceptanceNo: 101,
      deliveryDate: '2026-11-10',
      customerName: 'Customer A',
      receiveNo: 588354,
      productCode: 5180061,
      productName: 'Carton Box A',
      quantity: 100,
      stock: 50,
      currentStock: 200,
      actualLoading: 100,
      poNo: 'PO-001',
      remark: 'Urgent',
      sqm: 1.5,
      totalSqm: 150,
      flute: 'B',
      lastProcess: 'Printing',
      acceptanceNote: 'OK',
      dateInput: '2026-11-10T08:00:00',
      userFirstName: 'Thanh',
      dateEdit: null,
      userEditFirstName: null,
    },
  ],
};

describe('DeliveryPlanSearchPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryPlanSearchPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests delivery plan search with trimmed filters', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      report: () => DeliveryPlanSearchResult | null;
    };
    page.changeFilter('deliveryDate', ' 11/10/2026 ');
    page.changeFilter('receiveNo', ' 588354 ');
    page.search();

    const request = http.expectOne((req) => req.url === '/api/DeliveryPlan/search');
    expect(request.request.params.get('deliveryDate')).toBe('11/10/2026');
    expect(request.request.params.get('receiveNo')).toBe('588354');

    request.flush(mockResult);
    expect(page.report()).toEqual(mockResult);
    http.verify();
  });

  it('rejects invalid deliveryDate', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      error: () => string | null;
    };

    page.changeFilter('deliveryDate', '99/99/9999');
    page.search();
    expect(page.error()).toContain('valid date');

    http.expectNone('/api/DeliveryPlan/search');
    http.verify();
  });

  it('clears results and filters on clear()', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => DeliveryPlanSearchResult | null;
      filters: () => { receiveNo: string };
    };

    page.changeFilter('receiveNo', '588354');
    page.search();
    http.expectOne((req) => req.url === '/api/DeliveryPlan/search').flush(mockResult);
    expect(page.report()).toEqual(mockResult);

    page.clear();
    expect(page.report()).toBeNull();
    expect(page.filters().receiveNo).toBe('');
    http.verify();
  });
});
