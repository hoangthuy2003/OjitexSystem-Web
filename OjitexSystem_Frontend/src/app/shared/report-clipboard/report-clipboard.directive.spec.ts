import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { AgGridAngular } from 'ag-grid-angular';
import { ClientSideRowModelModule, ModuleRegistry, PaginationModule } from 'ag-grid-community';
import type { ColDef } from 'ag-grid-community';
import { ReportClipboardDirective } from './report-clipboard.directive';

ModuleRegistry.registerModules([ClientSideRowModelModule, PaginationModule]);

@Component({
  imports: [AgGridAngular, ReportClipboardDirective],
  template: `
    <ag-grid-angular
      reportClipboard
      [rowData]="items"
      [columnDefs]="columns"
      [pagination]="true"
      [paginationPageSize]="25"
      [paginationPageSizeSelector]="false"
      [enableCellTextSelection]="true"
    />
    <table reportClipboard>
      <tbody>
        <tr>
          <td>00123</td>
          <td data-clipboard-number="1420">1,420</td>
          <td>{{ note }}</td>
        </tr>
        <tr>
          <td>=A1</td>
          <td data-clipboard-number="0">0</td>
          <td></td>
        </tr>
      </tbody>
    </table>
  `,
})
class TestReport {
  readonly note = 'Line 1\nLine 2';
  readonly items = Array.from({ length: 60 }, (_, index) => ({
    code: `00${index}`,
    name: index === 0 ? 'A\t"B"\r\n<C>&D' : `Product ${index}`,
    qty: index === 0 ? 1420 : index,
  }));
  readonly columns: ColDef<{ code: string; name: string; qty: number }>[] = [
    { field: 'code', headerName: 'Code' },
    { field: 'name', headerName: 'Name' },
    {
      field: 'qty',
      headerName: 'Quantity',
      valueFormatter: ({ value }: { value: number }) => value.toLocaleString('en-US'),
    },
  ];
}

describe('report clipboard', () => {
  afterEach(() => window.getSelection()?.removeAllRanges());

  async function setup() {
    const fixture = TestBed.createComponent(TestReport);
    fixture.detectChanges();
    await fixture.whenStable();
    const grid = fixture.debugElement.query(By.directive(AgGridAngular));
    return {
      fixture,
      host: grid.nativeElement as HTMLElement,
      api: (grid.componentInstance as AgGridAngular).api,
      table: fixture.nativeElement.querySelector('table') as HTMLTableElement,
    };
  }

  function select(start: Node, end: Node, reverse = false) {
    const selection = window.getSelection()!;
    selection.removeAllRanges();
    const endOffset =
      end.nodeType === Node.TEXT_NODE ? end.textContent!.length : end.childNodes.length;
    selection.setBaseAndExtent(
      reverse ? end : start,
      reverse ? endOffset : 0,
      reverse ? start : end,
      reverse ? 0 : endOffset,
    );
  }

  function cell(host: HTMLElement, row: number, column: string) {
    // Only endpoints exist in the DOM: intervening rows/columns must come from the model.
    const element = document.createElement('div');
    element.className = 'ag-row';
    element.setAttribute('row-index', String(row));
    element.innerHTML = `<div class="ag-cell" col-id="${column}">Rendered text</div>`;
    host.append(element);
    return element.firstElementChild!.firstChild!;
  }

  function copy(host: HTMLElement) {
    const data = new Map<string, string>();
    const event = new Event('copy', { bubbles: true, cancelable: true });
    Object.defineProperty(event, 'clipboardData', {
      value: { setData: (type: string, value: string) => data.set(type, value) },
    });
    host.dispatchEvent(event);
    const html = document.createElement('div');
    html.innerHTML = data.get('text/html') ?? '';
    return { event, data, html };
  }

  it('copies all model rows and columns between endpoints, including unrendered data', async () => {
    const { host } = await setup();
    select(cell(host, 0, 'code'), cell(host, 59, 'qty'));
    const { data, html } = copy(host);
    expect(html.querySelectorAll('tr')).toHaveLength(60);
    expect(html.querySelectorAll('tr')[30].textContent).toBe('0030Product 3030');
    expect(html.querySelectorAll('tr')[59].lastElementChild?.textContent).toBe('59');
    expect(data.get('text/plain')).toContain('0059\tProduct 59\t59');
  });

  it('keeps tabs, quotes, line breaks, codes, quantities and escaped HTML inside their cells', async () => {
    const { host } = await setup();
    select(cell(host, 0, 'code'), cell(host, 0, 'qty'));
    const { data, html } = copy(host);
    expect(data.get('text/plain')).toBe('000\t"A\t""B""\r\n<C>&D"\t1,420');
    const cells = html.querySelectorAll('td');
    expect(cells).toHaveLength(3);
    expect(cells[0].getAttribute('style')).toContain('mso-number-format');
    expect(cells[1].querySelectorAll('br')).toHaveLength(1);
    expect(cells[1].querySelector('c')).toBeNull();
    expect(cells[1].textContent).toContain('<C>&D');
    expect(cells[2].getAttribute('x:num')).toBe('1420');
  });

  it('copies a reverse selection in the current column and sorted row order', async () => {
    const { host, api, fixture } = await setup();
    api.applyColumnState({
      state: [{ colId: 'qty', sort: 'desc' }, { colId: 'code' }, { colId: 'name' }],
      applyOrder: true,
    });
    await fixture.whenStable();
    select(cell(host, 0, 'qty'), cell(host, 2, 'code'), true);
    expect(copy(host).data.get('text/plain')).toBe('1,420\t000\r\n59\t0059\r\n58\t0058');
  });

  it('copies an entire single cell rather than a text fragment', async () => {
    const { host } = await setup();
    const node = cell(host, 2, 'code');
    select(node, node);
    expect(copy(host).data.get('text/plain')).toBe('002');
  });

  it('supports Ctrl+A then Ctrl+C across pages with headers, and resets on pointer selection', async () => {
    const { host } = await setup();
    host.dispatchEvent(
      new KeyboardEvent('keydown', { key: 'a', ctrlKey: true, bubbles: true, cancelable: true }),
    );
    host.dispatchEvent(new KeyboardEvent('keydown', { key: 'c', ctrlKey: true, bubbles: true }));
    const { html } = copy(host);
    expect(html.querySelectorAll('tr')).toHaveLength(61);
    expect(html.querySelector('tr')?.textContent).toBe('CodeNameQuantity');
    host.dispatchEvent(new Event('pointerdown', { bubbles: true }));
    const node = cell(host, 2, 'qty');
    select(node, node);
    expect(copy(host).data.get('text/plain')).toBe('2');
  });

  it('copies history as a rectangular table with multiline text and numeric quantities', async () => {
    const { table } = await setup();
    select(table.rows[0].cells[0].firstChild!, table.rows[1].cells[2]);
    const { data, html } = copy(table);
    expect(data.get('text/plain')).toBe('00123\t1,420\t"Line 1\nLine 2"\r\n=A1\t0\t');
    expect(html.querySelectorAll('td')).toHaveLength(6);
    expect(html.querySelectorAll('td')[1].getAttribute('x:num')).toBe('1420');
  });

  it('leaves copy in filter inputs and selections outside the table to the browser', async () => {
    const { host } = await setup();
    const input = document.createElement('input');
    host.append(input);
    const node = cell(host, 0, 'code');
    select(node, node);
    expect(copy(input).event.defaultPrevented).toBe(false);
    const outside = document.createTextNode('Outside');
    document.body.append(outside);
    select(outside, node);
    expect(copy(host).event.defaultPrevented).toBe(false);
    outside.remove();
  });
});
