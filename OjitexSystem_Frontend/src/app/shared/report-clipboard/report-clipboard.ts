import type { GridApi } from 'ag-grid-community';

type ReportGridApi = Pick<
  GridApi,
  'getAllDisplayedColumns' | 'getDisplayedRowAtIndex' | 'getDisplayedRowCount' | 'getCellValue'
>;

interface ClipboardCell {
  text: string;
  number?: number;
}

export function copyReportRange(
  event: ClipboardEvent,
  host: HTMLElement,
  api?: ReportGridApi,
  all = false,
): void {
  if (!event.clipboardData) return;
  const selection = host.ownerDocument.getSelection();
  if (!selection || selection.isCollapsed || !selection.rangeCount) return;
  const element = (node: Node | null) => (node instanceof Element ? node : node?.parentElement);
  const selector = api ? '.ag-cell[col-id], .ag-header-cell[col-id]' : 'td, th';
  const start = element(selection.anchorNode)?.closest<HTMLElement>(selector);
  const end = element(selection.focusNode)?.closest<HTMLElement>(selector);
  if (!all && (!start || !end || !host.contains(start) || !host.contains(end))) return;

  let rows: ClipboardCell[][];
  if (api) {
    const columns = api.getAllDisplayedColumns();
    const firstColumn = all
      ? 0
      : columns.findIndex((c) => c.getColId() === start!.getAttribute('col-id'));
    const lastColumn = all
      ? columns.length - 1
      : columns.findIndex((c) => c.getColId() === end!.getAttribute('col-id'));
    const rowIndex = (cell: HTMLElement) => {
      if (cell.matches('.ag-header-cell')) return -1;
      const index = cell.closest('.ag-row[row-index]')?.getAttribute('row-index');
      return index == null ? NaN : Number(index);
    };
    const startRow = all ? -1 : rowIndex(start!);
    const endRow = all ? api.getDisplayedRowCount() - 1 : rowIndex(end!);
    if (
      firstColumn < 0 ||
      lastColumn < 0 ||
      !Number.isInteger(startRow) ||
      !Number.isInteger(endRow)
    )
      return;
    const selectedColumns = columns.slice(
      Math.min(firstColumn, lastColumn),
      Math.max(firstColumn, lastColumn) + 1,
    );
    rows = [];
    for (let index = Math.min(startRow, endRow); index <= Math.max(startRow, endRow); index++) {
      if (index === -1) {
        rows.push(
          selectedColumns.map((column) => ({
            text: column.getColDef().headerName ?? column.getColId(),
          })),
        );
        continue;
      }
      // Read the sorted/filtered model, including cells outside the rendered viewport.
      const rowNode = api.getDisplayedRowAtIndex(index);
      if (!rowNode) return;
      rows.push(
        selectedColumns.map((column) => {
          const raw: unknown = api.getCellValue({ rowNode, colKey: column, useFormatter: false });
          const formatted: unknown = api.getCellValue({
            rowNode,
            colKey: column,
            useFormatter: true,
          });
          return {
            text: formatted == null ? '' : String(formatted),
            number: typeof raw === 'number' && Number.isFinite(raw) ? raw : undefined,
          };
        }),
      );
    }
  } else {
    if (!(host instanceof HTMLTableElement)) return;
    const tableRows = Array.from(host.rows);
    const startRow = tableRows.indexOf(start!.parentElement as HTMLTableRowElement);
    const endRow = tableRows.indexOf(end!.parentElement as HTMLTableRowElement);
    const startColumn = (start as HTMLTableCellElement).cellIndex;
    const endColumn = (end as HTMLTableCellElement).cellIndex;
    rows = tableRows.slice(Math.min(startRow, endRow), Math.max(startRow, endRow) + 1).map((row) =>
      Array.from(row.cells)
        .slice(Math.min(startColumn, endColumn), Math.max(startColumn, endColumn) + 1)
        .map((cell) => {
          const value = cell.getAttribute('data-clipboard-number');
          const number = value == null ? undefined : Number(value);
          return {
            text: cell.textContent ?? '',
            number: number !== undefined && Number.isFinite(number) ? number : undefined,
          };
        }),
    );
  }

  if (!rows.length) return;
  const quote = (text: string) =>
    /[\t\r\n"]/.test(text) ? `"${text.replaceAll('"', '""')}"` : text;
  const escape = (text: string) =>
    text
      .replaceAll('&', '&amp;')
      .replaceAll('<', '&lt;')
      .replaceAll('>', '&gt;')
      .replaceAll('"', '&quot;');
  const html = rows
    .map(
      (row) =>
        `<tr>${row
          .map((cell) => {
            const attributes =
              cell.number === undefined
                ? `style='mso-number-format:"\\@"'`
                : `x:num="${cell.number}"`;
            const content = escape(cell.text).replace(
              /\r\n|\r|\n/g,
              '<br style="mso-data-placement:same-cell">',
            );
            return `<td ${attributes}>${content}</td>`;
          })
          .join('')}</tr>`,
    )
    .join('');
  event.clipboardData.setData(
    'text/plain',
    rows.map((row) => row.map((cell) => quote(cell.text)).join('\t')).join('\r\n'),
  );
  event.clipboardData.setData(
    'text/html',
    `<html xmlns:x="urn:schemas-microsoft-com:office:excel"><head><meta charset="utf-8"></head><body><table>${html}</table></body></html>`,
  );
  event.preventDefault();
}
