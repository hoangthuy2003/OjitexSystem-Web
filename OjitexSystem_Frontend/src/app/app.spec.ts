import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { App } from './app';

describe('App', () => {
  let httpTesting: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTesting.verify());

  it('loads current stock and renders the returned product', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();

    const request = httpTesting.expectOne('/api/CurrentStock');
    expect(request.request.method).toBe('GET');
    request.flush([
      {
        cstProCd: 1234567,
        cstOpenStock: 10,
        cstWarehouseNg: 1,
        cstDisposal: 0,
        cstRepair: 2,
        cstProduction: 3,
        cstDelivery: 4,
        cstStock: 8,
      },
    ]);

    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Tồn kho hiện tại');
    expect(compiled.querySelector('.product-code')?.textContent).toContain('1.234.567');
    expect(compiled.textContent).toContain('Còn hàng');
  });

  it('loads a product detail by product code when selected', async () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    httpTesting.expectOne('/api/CurrentStock').flush([
      {
        cstProCd: 1234567,
        cstOpenStock: 10,
        cstWarehouseNg: 1,
        cstDisposal: 0,
        cstRepair: 2,
        cstProduction: 3,
        cstDelivery: 4,
        cstStock: 8,
      },
    ]);
    await fixture.whenStable();
    fixture.detectChanges();

    (fixture.nativeElement as HTMLElement).querySelector<HTMLButtonElement>('.product-code')?.click();
    const detailRequest = httpTesting.expectOne('/api/CurrentStock/1234567');
    expect(detailRequest.request.method).toBe('GET');
    detailRequest.flush({
      cstProCd: 1234567,
      cstOpenStock: 10,
      cstWarehouseNg: 1,
      cstDisposal: 0,
      cstRepair: 2,
      cstProduction: 3,
      cstDelivery: 4,
      cstStock: 8,
    });

    await fixture.whenStable();
    fixture.detectChanges();
    expect((fixture.nativeElement as HTMLElement).querySelector('.detail-code')?.textContent).toContain('1.234.567');
  });
});
