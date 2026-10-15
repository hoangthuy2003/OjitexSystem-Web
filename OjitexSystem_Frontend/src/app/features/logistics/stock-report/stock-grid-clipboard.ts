import { ColDef } from 'ag-grid-community';
import { StockReportItem } from './stock-report.models';

export function copyStockGridRange(
  event: ClipboardEvent,
  columnDefs: ColDef<StockReportItem>[],
): void {
  const selection = window.getSelection();
  const clipboard = event.clipboardData;
  const gridElement = event.currentTarget;
  if (!selection || !clipboard || !(gridElement instanceof HTMLElement)) return;

  const startCell = getSelectedCell(selection.anchorNode);
  const endCell = getSelectedCell(selection.focusNode);
  if (!startCell || !endCell || startCell === endCell) return;

  const startRow = startCell.closest<HTMLElement>('.ag-row[row-index]');
  const endRow = endCell.closest<HTMLElement>('.ag-row[row-index]');
  const startRowIndex = Number(startRow?.getAttribute('row-index'));
  const endRowIndex = Number(endRow?.getAttribute('row-index'));
  const startColumnId = startCell.getAttribute('col-id');
  const endColumnId = endCell.getAttribute('col-id');
  const columnIds = columnDefs
    .map(({ field }) => field)
    .filter((field): field is keyof StockReportItem & string => field !== undefined);
  const startColumnIndex = columnIds.findIndex((columnId) => columnId === startColumnId);
  const endColumnIndex = columnIds.findIndex((columnId) => columnId === endColumnId);

  if (
    !Number.isInteger(startRowIndex) ||
    !Number.isInteger(endRowIndex) ||
    startColumnIndex < 0 ||
    endColumnIndex < 0
  ) {
    return;
  }

  const firstRow = Math.min(startRowIndex, endRowIndex);
  const lastRow = Math.max(startRowIndex, endRowIndex);
  const firstColumn = Math.min(startColumnIndex, endColumnIndex);
  const lastColumn = Math.max(startColumnIndex, endColumnIndex);
  const rows = Array.from(gridElement.querySelectorAll<HTMLElement>('.ag-row[row-index]'))
    .filter((row) => {
      const rowIndex = Number(row.getAttribute('row-index'));
      return rowIndex >= firstRow && rowIndex <= lastRow;
    })
    .sort(
      (left, right) =>
        Number(left.getAttribute('row-index')) - Number(right.getAttribute('row-index')),
    );

  const text = rows
    .map((row) => {
      const cells = new Map(
        Array.from(row.querySelectorAll<HTMLElement>('.ag-cell[col-id]')).map((cell) => [
          cell.getAttribute('col-id'),
          cell.textContent?.trim() ?? '',
        ]),
      );
      return columnIds
        .slice(firstColumn, lastColumn + 1)
        .map((columnId) => cells.get(columnId) ?? '')
        .join('\t');
    })
    .join('\n');

  if (!text) return;
  event.preventDefault();
  clipboard.setData('text/plain', text);
}

function getSelectedCell(node: Node | null): HTMLElement | null {
  const element = node instanceof Element ? node : node?.parentElement;
  return element?.closest<HTMLElement>('.ag-cell[col-id]') ?? null;
}
