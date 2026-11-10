import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { GoodsReceiptItem } from '../goods-receipt-search/goods-receipt-search.models';

function formatText(value: unknown, code = false): string {
  if (value == null) return '—';
  if (typeof value === 'string') {
    const trimmed = value.trim();
    return trimmed || '—';
  }
  if (typeof value === 'number' && !code) {
    return value.toLocaleString('en-US', { maximumFractionDigits: 3 });
  }
  return String(value);
}

export const GOODS_RECEIPT_INFORMATION_DEFAULT_COLUMN: ColDef<GoodsReceiptItem> = {
  flex: 1,
  minWidth: 70,
  resizable: true,
  sortable: true,
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'left' },
  valueFormatter: ({ value }) => formatText(value),
};

export const GOODS_RECEIPT_INFORMATION_COLUMN_DEFS: ColDef<GoodsReceiptItem>[] = [
  {
    field: 'dateReceipt',
    headerName: 'Date Receipt',
    minWidth: 105,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'MM/dd/yyyy', 'en-US') : '—'),
  },
  {
    field: 'receiveNo',
    headerName: 'Receive no',
    minWidth: 100,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'customerCode',
    headerName: 'Customer Code',
    minWidth: 115,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'customerName',
    headerName: 'Customer Name',
    minWidth: 160,
    tooltipField: 'customerName',
  },
  {
    field: 'productCode',
    headerName: 'Product Code',
    minWidth: 110,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'productName',
    headerName: 'Product Name',
    minWidth: 260,
    tooltipField: 'productName',
  },
  {
    field: 'quantity',
    headerName: 'Quantity',
    minWidth: 90,
    cellStyle: { textAlign: 'left' },
  },
  {
    field: 'productionSection',
    headerName: 'Section',
    minWidth: 85,
  },
  {
    field: 'note',
    headerName: 'Note',
    minWidth: 120,
    tooltipField: 'note',
  },
  {
    field: 'dateInput',
    headerName: 'Date Input',
    minWidth: 140,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'userFirstName',
    headerName: 'User Input',
    minWidth: 100,
  },
  {
    field: 'dateChanged',
    headerName: 'Date Changed',
    minWidth: 140,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'userChangedFirstName',
    headerName: 'User Changed',
    minWidth: 100,
  },
];
