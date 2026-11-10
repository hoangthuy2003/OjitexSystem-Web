import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryPlanMonthlyReport } from './delivery-plan-monthly.models';
import { DeliveryPlanMonthlyPage } from './delivery-plan-monthly-page';

const mockReport: DeliveryPlanMonthlyReport = {
  items: [
    {
      acceptanceNo: 201,
      deliveryDate: '2026-11-05',
      customerName: 'Customer Monthly',
      receiveNo: 654321,
      productCode: 2001,
      productName: 'Box Monthly',
      quantity: 200,
      stock: 150,
      currentStock: 350,
      actualLoading: 200,
      poNo: 'PO-M1',
      remark: 'Monthly Remark',
      sqm: 2.0,
      totalSqm: 400,
      flute: 'B',
      lastProcess: 'Cut',
      dateInput: '2026-11-01T09:00:00',
      dateEdit: '2026-11-02T09:00:00',
      userName: 'Ms. Monthly',
    },
  ],
};

describe('DeliveryPlanMonthlyPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryPlanMonthlyPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests delivery plan monthly with default dates and optional product code', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      report: () => DeliveryPlanMonthlyReport | null;
    };

    page.changeFilter('productCode', '2001');
    page.search();

    const request = http.expectOne((req) => req.url === '/api/DeliveryPlan/monthly');
    expect(request.request.params.get('productCode')).toBe('2001');
    expect(request.request.params.get('fromDate')).toMatch(/^\d{2}\/\d{2}\/\d{4}$/);
    expect(request.request.params.get('toDate')).toMatch(/^\d{2}\/\d{2}\/\d{4}$/);

    request.flush(mockReport);
    expect(page.report()).toEqual(mockReport);
    http.verify();
  });

  it('validates invalid date and chronological order', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      error: () => string | null;
    };

    page.changeFilter('fromDate', '99/99/9999');
    page.search();
    expect(page.error()).toContain('valid from date');

    page.changeFilter('fromDate', '11/10/2026');
    page.changeFilter('toDate', '11/01/2026');
    page.search();
    expect(page.error()).toContain('earlier than or equal to');

    http.expectNone('/api/DeliveryPlan/monthly');
    http.verify();
  });

  it('clears product code and restores default dates on clear()', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => DeliveryPlanMonthlyReport | null;
      filters: () => { fromDate: string; toDate: string; productCode: string };
    };

    page.changeFilter('productCode', '2001');
    page.search();
    http.expectOne((req) => req.url === '/api/DeliveryPlan/monthly').flush(mockReport);
    expect(page.report()).toEqual(mockReport);

    page.clear();
    expect(page.report()).toBeNull();
    expect(page.filters().productCode).toBe('');
    expect(page.filters().fromDate).toMatch(/^\d{2}\/\d{2}\/\d{4}$/);
    http.verify();
  });
});
