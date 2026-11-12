import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { PaperRollImportSearchResult } from './paper-roll-import-search.models';
import { PaperRollImportSearchPage } from './paper-roll-import-search-page';

const mockResult: PaperRollImportSearchResult = {
  items: [
    {
      serialNo: 'C16076973A',
      paperLabel: '26KV0691',
      gradeName: 'FMVIK',
      grammage: 175,
      paperWidth: 1900,
      importKg: 2074,
      paperType: 'N',
      supplierName: 'VINA KRAFT',
      containerNo: 'HACU4220857',
      dateReceived: '2026-11-11T00:00:00',
      dateInput: '2026-11-12T08:32:00',
      dateEdit: '2026-11-12T08:32:00',
      userInput: 'Ms. Thu',
      remark: null,
    },
  ],
};

describe('PaperRollImportSearchPage', () => {
  function setup() {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    const fixture = TestBed.createComponent(PaperRollImportSearchPage);
    fixture.detectChanges();
    const page = fixture.componentInstance as unknown as {
      changeFilter: (field: string, value: string) => void;
      search: () => void;
      clear: () => void;
      report: () => PaperRollImportSearchResult | null;
      error: () => string | null;
      filters: () => { paperLabel: string; gradeName: string; dateReceived: string };
    };
    return { fixture, http: TestBed.inject(HttpTestingController), page };
  }

  it('searches with trimmed label, grade and received date filters', () => {
    const { fixture, http, page } = setup();
    page.changeFilter('paperLabel', ' 26KV ');
    page.changeFilter('gradeName', ' FM ');
    page.changeFilter('dateReceived', '11/11/2026');
    page.search();

    const request = http.expectOne((req) => req.url === '/api/PaperRollImport/search');
    expect(request.request.params.get('paperLabel')).toBe('26KV');
    expect(request.request.params.get('gradeName')).toBe('FM');
    expect(request.request.params.get('dateReceived')).toBe('11/11/2026');
    request.flush(mockResult);
    expect(page.report()).toEqual(mockResult);
    fixture.detectChanges();
    expect((fixture.nativeElement as HTMLElement).textContent).toContain(
      'PAPER ROLL IMPORT SEARCH',
    );
    http.verify();
  });

  it('rejects invalid date received before sending the request', () => {
    const { http, page } = setup();
    page.changeFilter('dateReceived', '13/40/2026');
    page.search();
    expect(page.error()).toContain('valid date');
    http.expectNone('/api/PaperRollImport/search');
    http.verify();
  });

  it('clears filters and search results', () => {
    const { http, page } = setup();
    page.changeFilter('paperLabel', '26KV0691');
    page.search();
    http.expectOne((req) => req.url === '/api/PaperRollImport/search').flush(mockResult);
    page.clear();
    expect(page.filters()).toEqual({ paperLabel: '', gradeName: '', dateReceived: '' });
    expect(page.report()).toBeNull();
    http.verify();
  });
});
