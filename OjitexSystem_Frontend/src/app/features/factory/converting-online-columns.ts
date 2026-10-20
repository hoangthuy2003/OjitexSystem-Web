import { ColDef } from 'ag-grid-community';
import { ConvertingOnlineItem } from './converting-online.models';

interface ConvertingColumn {
  field: keyof ConvertingOnlineItem;
  label: string;
  minWidth: number;
  code?: boolean;
}

export const CONVERTING_COLUMNS: readonly ConvertingColumn[] = [
  { field: 'shift', label: 'Shift', minWidth: 50 },
  // Keep the Excel labels and SQL field order: lot number precedes received order number.
  { field: 'lotNo', label: 'Order No.', minWidth: 66, code: true },
  { field: 'orderNo', label: 'Receive No.', minWidth: 76, code: true },
  { field: 'customerCode', label: 'Customer Code', minWidth: 82, code: true },
  { field: 'customerName', label: 'Customer Name', minWidth: 115 },
  { field: 'productCode', label: 'Product Code', minWidth: 86, code: true },
  { field: 'productName', label: 'Product Name', minWidth: 180 },
  { field: 'planQty', label: 'Plan Qty', minWidth: 64 },
  { field: 'inputQty', label: 'Input Qty', minWidth: 64 },
  { field: 'outputQty', label: 'Output Qty', minWidth: 68 },
  { field: 'surplusQty', label: 'Surplus Qty', minWidth: 68 },
  { field: 'lossQty', label: 'Loss Qty', minWidth: 64 },
  { field: 'shortestQty', label: 'Shortest Qty', minWidth: 76 },
  { field: 'startTime', label: 'Start Time', minWidth: 70 },
  { field: 'finishTime', label: 'Finish Time', minWidth: 70 },
  { field: 'drivingMinutes', label: 'Driving Time (Minutes)', minWidth: 88 },
  { field: 'setMinutes', label: 'Set Time (Minutes)', minWidth: 88 },
  { field: 'stopMinutes', label: 'Stop Time (Minutes)', minWidth: 88 },
  { field: 'restMinutes', label: 'Rest Time (Minutes)', minWidth: 88 },
];

export const CONVERTING_DEFAULT_COLUMN: ColDef<ConvertingOnlineItem> = {
  flex: 1,
  minWidth: 50,
  resizable: true,
  sortable: true,
  headerClass: 'center-header',
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'center' },
};

export const CONVERTING_COLUMN_DEFS: ColDef<ConvertingOnlineItem>[] = CONVERTING_COLUMNS.map(
  (column) => ({
    field: column.field,
    headerName: column.label,
    minWidth: column.minWidth,
    tooltipField:
      column.field === 'productName' || column.field === 'customerName' ? column.field : undefined,
    valueFormatter: ({ value }) => {
      return formatConvertingValue(value, column.code);
    },
  }),
);

export function formatConvertingValue(value: unknown, code = false): string {
  if (value == null || value === '') return '—';
  return typeof value === 'number' && !code ? value.toLocaleString() : String(value);
}
