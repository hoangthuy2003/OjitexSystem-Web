import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryInformationReport } from '../delivery-note-search/delivery-note-search.models';
import { DeliveryInformationPage } from './delivery-information-page';

const report: DeliveryInformationReport = {
  items: [
    {
      key: 1,
      lineNo: 1,
      deliverySlip: '71123',
      receiveNo: 0,
      customerCode: 400,
      customerName: 'CHI NHANH CONG TY TNHH',
      productCode: 4001000,
      productName: '30004804 - Thung CAN 250 ml',
      quantity: 16800,
      deliveryDate: '2026-11-05',
      customerPo: null,
      remark: null,
      otherNote: null,
      truckNumber: '29G01876',
      carType: '15',
      transportCompany: 'NTT',
      dateInput: '2026-11-05T08:04:00',
      userId: 'long',
      userFirstName: 'Mr. Long',
      state: 'Inserted',
      flute: '-B',
      sheetSqm: 0.415,
      totalSqm: 6972,
    },
  ],
};

describe('DeliveryInformationPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryInformationPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('requests delivery information with trimmed filters', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      report: () => DeliveryInformationReport | null;
    };
    page.changeFilter('deliverySlip', ' 71123 ');
    page.changeFilter('customerCode', ' 400 ');
    page.changeFilter('productCode', ' 4001000 ');
    page.changeFilter('deliveryDate', ' 11/05/2026 ');
    page.search();

    const req = http.expectOne(
      '/api/DeliveryNote/information?deliverySlip=71123&customerCode=400&productCode=4001000&deliveryDate=11/05/2026',
    );
    req.flush(report);
    expect(page.report()?.items).toHaveLength(1);
    http.verify();
  });

  it('validates invalid date format before calling API', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      error: () => string | null;
    };
    page.changeFilter('deliveryDate', '2026-11-05');
    page.search();
    expect(page.error()).toContain('mm/dd/yyyy');
    http.expectNone('/api/DeliveryNote/information');
    http.verify();
  });

  it('loads all records when Show All is clicked and resets on Clear', () => {
    const { fixture, http } = setup();
    const page = fixture.componentInstance as unknown as {
      showAll: () => void;
      clear: () => void;
      report: () => DeliveryInformationReport | null;
    };
    page.showAll();
    http.expectOne('/api/DeliveryNote/information').flush(report);
    expect(page.report()?.items).toHaveLength(1);
    page.clear();
    expect(page.report()).toBeNull();
    http.verify();
  });
});

