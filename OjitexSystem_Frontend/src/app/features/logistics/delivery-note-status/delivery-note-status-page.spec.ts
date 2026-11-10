import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryStatusReport } from './delivery-note-status.models';
import { DeliveryNoteStatusPage } from './delivery-note-status-page';

const mockReport: DeliveryStatusReport = {
  items: [
    {
      key: 1,
      customerCode: 101,
      customerName: 'Customer A',
      deliveryDate: '2026-11-08',
      transportSupplier: 'Transport Co A',
      truckNumber: '29C-12345',
      deliverySlip: 'SLIP-01',
      status: 'Delivered',
      remark: 'On time',
    },
  ],
};

describe('DeliveryNoteStatusPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryNoteStatusPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests delivery note status with default dates', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      search: () => void;
      report: () => DeliveryStatusReport | null;
    };

    page.search();

    const request = http.expectOne((req) => req.url === '/api/DeliveryNote/status');
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

    http.expectNone('/api/DeliveryNote/status');
    http.verify();
  });

  it('restores default dates and clears report on clear()', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => DeliveryStatusReport | null;
      filters: () => { fromDate: string; toDate: string };
    };

    page.changeFilter('fromDate', '11/05/2026');
    page.search();
    http.expectOne((req) => req.url === '/api/DeliveryNote/status').flush(mockReport);
    expect(page.report()).toEqual(mockReport);

    page.clear();
    expect(page.report()).toBeNull();
    expect(page.filters().fromDate).toMatch(/^\d{2}\/01\/\d{4}$/);
    http.verify();
  });
});
