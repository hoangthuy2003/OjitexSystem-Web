import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { CONVERTING_COLUMNS, formatConvertingValue } from '../converting-online-columns';
import { ConvertingHistoryItem } from './converting-history.models';

const detailColumns: ColDef<ConvertingHistoryItem>[] = CONVERTING_COLUMNS.map((column) => ({
  field: column.field,
  headerName: column.label,
  minWidth: column.minWidth,
  tooltipField:
    column.field === 'productName' || column.field === 'customerName' ? column.field : undefined,
  valueFormatter: ({ value }) => formatConvertingValue(value, column.code),
}));

export const CONVERTING_HISTORY_COLUMN_DEFS: ColDef<ConvertingHistoryItem>[] = [
  {
    field: 'date',
    headerName: 'Date',
    minWidth: 100,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'MM/dd/yyyy', 'en-US') : '—'),
  },
  detailColumns[0],
  { field: 'machineName', headerName: 'Machine Name', minWidth: 88 },
  ...detailColumns.slice(1),
];

export const CONVERTING_HISTORY_DEFAULT_COLUMN: ColDef<ConvertingHistoryItem> = {
  flex: 1,
  minWidth: 50,
  resizable: true,
  sortable: true,
  headerClass: 'center-header',
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'center' },
  valueFormatter: ({ value }) => formatConvertingValue(value),
};
