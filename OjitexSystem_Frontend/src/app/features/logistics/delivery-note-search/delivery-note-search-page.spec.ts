import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeliveryNoteSearchPage } from './delivery-note-search-page';
import { DeliveryNote } from './delivery-note-search.models';

const note: DeliveryNote = {
  header: {
    key: 1,
    deliverySlip: '27544',
    customerCode: 115,
    customerName: 'Customer',
    transportCompany: 'NTT',
    truckNumber: '29H87890',
    carType: '10',
    deliveryDate: '2025-02-26',
    state: 'Updated',
    supplierCode: null,
    note: null,
    deliveryAddress: 'Ha Noi',
    goodsType: 'Finished Goods',
    dateChange: null,
    dateInput: '2025-02-26T13:03:00',
    userId: 'hung',
    userFirstName: 'Mr. Hung',
  },
  details: [
    {
      key: 1,
      deliverySlip: '27544',
      lineNo: 1,
      receiveNo: 0,
      productCode: 1150706,
      productName: 'Box',
      quantity: 20,
      customerPo: 'PO-1',
      remark: null,
      otherNote: null,
    },
  ],
};

describe('DeliveryNoteSearchPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(DeliveryNoteSearchPage);
    fixture.detectChanges();
    return { fixture, http: TestBed.inject(HttpTestingController) };
  }

  it('renders header and detail lines for the searched slip', async () => {
    const { fixture, http } = setup();
    fixture.componentInstance.deliverySlip = ' 27544 ';
    fixture.componentInstance.search();
    http.expectOne('/api/DeliveryNote/27544').flush(note);
    fixture.detectChanges();
    const text = (fixture.nativeElement as HTMLElement).textContent ?? '';
    expect(text).toContain('Mr. Hung');
    expect(text).toContain('1150706');
    http.verify();
  });

  it('pages detail lines 15 at a time', () => {
    const { fixture, http } = setup();
    const details = Array.from({ length: 23 }, (_, index) => ({
      ...note.details[0],
      key: index + 1,
      lineNo: index + 1,
    }));
    fixture.componentInstance.deliverySlip = '27544';
    fixture.componentInstance.search();
    http.expectOne('/api/DeliveryNote/27544').flush({ ...note, details });
    expect(fixture.componentInstance.pageCount).toBe(2);
    expect(fixture.componentInstance.pagedDetails).toHaveLength(15);
    fixture.componentInstance.goToPage(1);
    expect(fixture.componentInstance.pagedDetails.map((detail) => detail.lineNo)).toEqual([
      16, 17, 18, 19, 20, 21, 22, 23,
    ]);
    fixture.componentInstance.goToPage(5);
    expect(fixture.componentInstance.pageIndex).toBe(1);
  });

  it('shows a not-found message and clears the content', () => {
    const { fixture, http } = setup();
    fixture.componentInstance.deliverySlip = '1';
    fixture.componentInstance.search();
    http
      .expectOne('/api/DeliveryNote/1')
      .flush('missing', { status: 404, statusText: 'Not Found' });
    expect(fixture.componentInstance.errorMessage).toContain('No delivery note');
    fixture.componentInstance.clear();
    expect(fixture.componentInstance.errorMessage).toBeNull();
    expect(fixture.componentInstance.note).toBeNull();
  });
});
