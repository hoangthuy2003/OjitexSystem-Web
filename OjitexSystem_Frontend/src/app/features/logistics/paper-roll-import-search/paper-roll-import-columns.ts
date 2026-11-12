import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { PaperRollImportItem } from './paper-roll-import-search.models';

function formatText(value: unknown): string {
  if (value == null) return '—';
  return typeof value === 'string' ? value.trim() || '—' : String(value);
}

function formatNumber(value: unknown): string {
  if (value == null) return '—';
  return typeof value === 'number' ? value.toLocaleString('en-US') : String(value);
}

export const PAPER_ROLL_IMPORT_DEFAULT_COLUMN: ColDef<PaperRollImportItem> = {
  flex: 1,
  minWidth: 90,
  resizable: true,
  sortable: true,
  wrapHeaderText: true,
  autoHeaderHeight: true,
  valueFormatter: ({ value }) => formatText(value),
};

export const PAPER_ROLL_IMPORT_COLUMN_DEFS: ColDef<PaperRollImportItem>[] = [
  {
    field: 'serialNo',
    headerName: 'Serial No',
    minWidth: 115,
  },
  {
    field: 'paperLabel',
    headerName: 'Paper Label',
    minWidth: 110,
  },
  {
    field: 'gradeName',
    headerName: 'Grade Name',
    minWidth: 110,
  },
  {
    field: 'grammage',
    headerName: 'Gramage',
    minWidth: 90,
    valueFormatter: ({ value }) => formatNumber(value),
  },
  {
    field: 'paperWidth',
    headerName: 'Paper Width',
    minWidth: 100,
    valueFormatter: ({ value }) => formatNumber(value),
  },
  {
    field: 'importKg',
    headerName: 'Import (Kg)',
    minWidth: 95,
    valueFormatter: ({ value }) => formatNumber(value),
  },
  {
    field: 'paperType',
    headerName: 'Paper Type',
    minWidth: 90,
  },
  {
    field: 'supplierName',
    headerName: 'Supplier Name',
    minWidth: 125,
    tooltipField: 'supplierName',
  },
  {
    field: 'containerNo',
    headerName: 'Container No',
    minWidth: 125,
  },
  {
    field: 'dateReceived',
    headerName: 'Date Received',
    minWidth: 115,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy', 'en-US') : '—'),
  },
  {
    field: 'dateInput',
    headerName: 'Date Input',
    minWidth: 145,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'dateEdit',
    headerName: 'Date Edit',
    minWidth: 145,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'userInput',
    headerName: 'User Input',
    minWidth: 110,
  },
  {
    field: 'remark',
    headerName: 'Remark',
    minWidth: 140,
    tooltipField: 'remark',
  },
];
