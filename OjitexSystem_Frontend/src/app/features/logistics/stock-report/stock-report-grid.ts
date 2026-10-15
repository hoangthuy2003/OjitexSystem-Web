import { ColDef, ValueFormatterParams } from 'ag-grid-community';
import { StockReportItem, StockReportKind } from './stock-report.models';

export const STOCK_REPORT_TITLES: Record<StockReportKind, string> = {
  current: 'Current Stock',
  last: 'Last Stock',
  monthly: 'Monthly Stock',
};

export const STOCK_DEFAULT_COLUMN: ColDef<StockReportItem> = {
  flex: 1,
  minWidth: 120,
  resizable: true,
  sortable: true,
  headerClass: 'center-header',
  cellStyle: { textAlign: 'center' },
};

function formatQuantity({ value }: ValueFormatterParams<StockReportItem, number | null>): string {
  return value == null ? '—' : value.toLocaleString();
}

export function stockReportColumns(kind: StockReportKind): ColDef<StockReportItem>[] {
  const quantities: [keyof StockReportItem, string][] = [
    ['openingStock', 'Opening Stock'],
    ['warehouseNg', 'NG Warehouse'],
    ['disposal', 'Disposal'],
    ['repair', 'Repair'],
    ['production', 'Production'],
    ['delivery', 'Delivery'],
  ];
  return [
    ...(kind === 'monthly'
      ? [
          {
            field: 'reportDate',
            headerName: 'Date',
            valueFormatter: ({ value }) =>
              value ? new Date(value + 'T00:00:00').toLocaleDateString('en-US') : '—',
          } satisfies ColDef<StockReportItem>,
        ]
      : []),
    { field: 'productCode', headerName: 'Product Code' },
    ...quantities.map(([field, headerName]) => ({
      field,
      headerName,
      cellClass: 'numeric-cell',
      valueFormatter: formatQuantity,
    })),
    {
      field: 'closingStock',
      headerName: { current: 'Current Stock', last: 'Stock', monthly: 'Closing Stock' }[kind],
      cellClass: 'numeric-cell stock-history-cell',
      tooltipValueGetter: () => 'Double-click to view stock history',
      valueFormatter: formatQuantity,
    },
  ];
}
