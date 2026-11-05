import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { DeliveryInformationItem } from '../delivery-note-search/delivery-note-search.models';

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

export const DELIVERY_INFORMATION_DEFAULT_COLUMN: ColDef<DeliveryInformationItem> = {
  flex: 1,
  minWidth: 70,
  resizable: true,
  sortable: true,
  headerClass: 'center-header',
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'left' },
  valueFormatter: ({ value }) => formatText(value),
};

export const DELIVERY_INFORMATION_COLUMN_DEFS: ColDef<DeliveryInformationItem>[] = [
  {
    field: 'lineNo',
    headerName: 'Line No',
    minWidth: 74,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'deliverySlip',
    headerName: 'Delivery Slip',
    minWidth: 96,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'receiveNo',
    headerName: 'Receive No',
    minWidth: 88,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'customerCode',
    headerName: 'Customer Code',
    minWidth: 100,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'customerName',
    headerName: 'Name',
    minWidth: 160,
    tooltipField: 'customerName',
  },
  {
    field: 'productCode',
    headerName: 'Product Code',
    minWidth: 100,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'productName',
    headerName: 'Product Name',
    minWidth: 220,
    tooltipField: 'productName',
  },
  {
    field: 'quantity',
    headerName: 'Qty',
    minWidth: 80,
  },
  {
    field: 'deliveryDate',
    headerName: 'Delivery',
    minWidth: 100,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'MM/dd/yyyy', 'en-US') : '—'),
  },
  {
    field: 'customerPo',
    headerName: 'PO',
    minWidth: 120,
    tooltipField: 'customerPo',
  },
  {
    field: 'remark',
    headerName: 'Remark',
    minWidth: 120,
    tooltipField: 'remark',
  },
  {
    field: 'otherNote',
    headerName: 'Logistics Note',
    minWidth: 120,
    tooltipField: 'otherNote',
  },
  {
    field: 'truckNumber',
    headerName: 'Truck Number',
    minWidth: 104,
  },
  {
    field: 'carType',
    headerName: 'Car Type',
    minWidth: 78,
  },
  {
    field: 'transportCompany',
    headerName: 'Transport',
    minWidth: 96,
  },
  {
    field: 'dateInput',
    headerName: 'Date Input',
    minWidth: 132,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy H:mm', 'en-US') : '—'),
  },
  {
    field: 'userFirstName',
    headerName: 'User Input',
    minWidth: 96,
  },
  {
    field: 'state',
    headerName: 'State',
    minWidth: 88,
  },
  {
    field: 'flute',
    headerName: 'Flute',
    minWidth: 68,
  },
  {
    field: 'sheetSqm',
    headerName: 'SQM',
    minWidth: 74,
  },
  {
    field: 'totalSqm',
    headerName: 'Total SQM',
    minWidth: 92,
  },
];

