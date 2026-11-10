import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { DeliveryPlanItem } from './delivery-plan-search.models';

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

export const DELIVERY_PLAN_SEARCH_DEFAULT_COLUMN: ColDef<DeliveryPlanItem> = {
  flex: 1,
  minWidth: 70,
  resizable: true,
  sortable: true,
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'left' },
  valueFormatter: ({ value }) => formatText(value),
};

export const DELIVERY_PLAN_SEARCH_COLUMN_DEFS: ColDef<DeliveryPlanItem>[] = [
  // {
  //   field: 'acceptanceNo',
  //   headerName: 'Acceptance No',
  //   minWidth: 80,
  //   valueFormatter: ({ value }) => formatText(value, true),
  // },
  {
    field: 'deliveryDate',
    headerName: 'Delivery Date',
    minWidth: 105,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy', 'en-US') : '—'),
  },
  {
    field: 'customerName',
    headerName: 'Customer Name',
    minWidth: 160,
    tooltipField: 'customerName',
  },
  {
    field: 'receiveNo',
    headerName: 'Receive No',
    minWidth: 100,
    valueFormatter: ({ value }) => formatText(value, true),
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
    field: 'stock',
    headerName: 'Stock',
    minWidth: 80,
    cellStyle: { textAlign: 'left' },
  },
  {
    field: 'currentStock',
    headerName: 'Current Stock',
    minWidth: 100,
    cellStyle: { textAlign: 'left' },
  },
  {
    field: 'actualLoading',
    headerName: 'Actual Loading',
    minWidth: 105,
    cellStyle: { textAlign: 'left' },
  },
  {
    field: 'poNo',
    headerName: 'PO No',
    minWidth: 120,
    tooltipField: 'poNo',
  },
  {
    field: 'remark',
    headerName: 'Remark',
    minWidth: 120,
    tooltipField: 'remark',
  },
  {
    field: 'sqm',
    headerName: 'SQM',
    minWidth: 80,
  },
  {
    field: 'totalSqm',
    headerName: 'Total SQM',
    minWidth: 90,
  },
  {
    field: 'flute',
    headerName: 'Flute',
    minWidth: 70,
  },
  {
    field: 'lastProcess',
    headerName: 'Last Process',
    minWidth: 100,
  },
  {
    field: 'acceptanceNote',
    headerName: 'Acceptance Note',
    minWidth: 130,
    tooltipField: 'acceptanceNote',
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
    field: 'dateEdit',
    headerName: 'Date Edit',
    minWidth: 140,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'userEditFirstName',
    headerName: 'User Edit',
    minWidth: 100,
  },
];
