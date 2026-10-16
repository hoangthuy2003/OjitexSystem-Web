import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { CorrugatorOnlineItem } from './corrugator-online.models';

interface CorrugatorColumn {
  field: keyof CorrugatorOnlineItem;
  label: string;
  format?: 'date' | 'datetime';
  minWidth: number;
}

export const CORRUGATOR_COLUMNS: readonly CorrugatorColumn[] = [
  { field: 'planDate', label: 'Date of Plan', format: 'date', minWidth: 96 },
  { field: 'orderNo', label: 'Order No.', minWidth: 66 },
  { field: 'targetCut', label: 'Target Cut', minWidth: 70 },
  { field: 'qualityItem', label: 'Quality Item', minWidth: 72 },
  { field: 'paperWidth', label: 'Paper Width', minWidth: 72 },
  { field: 'corruLength', label: 'Corru Length', minWidth: 72 },
  { field: 'sheetWidth', label: 'Sheet Width', minWidth: 72 },
  { field: 'corruUp', label: 'Corru UP', minWidth: 62 },
  { field: 'flute', label: 'Flute', minWidth: 50 },
  { field: 'glue', label: 'Glue', minWidth: 78 },
  { field: 'bm', label: 'BM', minWidth: 78 },
  { field: 'bl', label: 'BL', minWidth: 78 },
  { field: 'am', label: 'AM', minWidth: 78 },
  { field: 'al', label: 'AL', minWidth: 78 },
  { field: 'driveMinutes', label: 'Drive Time (Minutes)', minWidth: 88 },
  { field: 'stopMinutes', label: 'Stop Time (Minutes)', minWidth: 88 },
  { field: 'averageSpeed', label: 'AV Speed', minWidth: 70 },
  { field: 'endTime', label: 'End Time', minWidth: 70 },
  { field: 'corrugatedDate', label: 'Corrugated Date', format: 'date', minWidth: 96 },
  { field: 'dateInput', label: 'Date Input', format: 'datetime', minWidth: 140 },
];

export const CORRUGATOR_DEFAULT_COLUMN: ColDef<CorrugatorOnlineItem> = {
  flex: 1,
  minWidth: 50,
  resizable: true,
  sortable: true,
  headerClass: 'center-header',
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'center' },
};

export const CORRUGATOR_COLUMN_DEFS: ColDef<CorrugatorOnlineItem>[] = CORRUGATOR_COLUMNS.map(
  (column) => ({
    field: column.field,
    headerName: column.label,
    minWidth: column.minWidth,
    valueFormatter: ({ value }) => {
      if (value == null || value === '') return '—';
      if (column.format) {
        return formatDate(
          value,
          column.format === 'date' ? 'MM/dd/yyyy' : 'MM/dd/yyyy H:mm',
          'en-US',
        );
      }
      return typeof value === 'number' ? value.toLocaleString() : String(value);
    },
  }),
);
