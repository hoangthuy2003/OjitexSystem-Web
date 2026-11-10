import { formatDate } from '@angular/common';
import { ColDef } from 'ag-grid-community';
import { DeliveryStatusItem } from './delivery-note-status.models';

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

export const DELIVERY_NOTE_STATUS_DEFAULT_COLUMN: ColDef<DeliveryStatusItem> = {
  flex: 1,
  minWidth: 90,
  resizable: true,
  sortable: true,
  wrapHeaderText: true,
  autoHeaderHeight: true,
  cellStyle: { textAlign: 'left' },
  valueFormatter: ({ value }) => formatText(value),
};

export const DELIVERY_NOTE_STATUS_COLUMN_DEFS: ColDef<DeliveryStatusItem>[] = [
  {
    field: 'customerCode',
    headerName: 'Customer Code',
    minWidth: 120,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'customerName',
    headerName: 'Customer Name',
    minWidth: 200,
    tooltipField: 'customerName',
  },
  {
    field: 'deliveryDate',
    headerName: 'Delivery Date',
    minWidth: 120,
    valueFormatter: ({ value }) => (value ? formatDate(value, 'M/d/yyyy', 'en-US') : '—'),
  },
  {
    field: 'transportSupplier',
    headerName: 'Transport Supplier',
    minWidth: 150,
    tooltipField: 'transportSupplier',
  },
  {
    field: 'truckNumber',
    headerName: 'Truck Number',
    minWidth: 120,
    tooltipField: 'truckNumber',
  },
  {
    field: 'deliverySlip',
    headerName: 'Delivery Slip',
    minWidth: 120,
    valueFormatter: ({ value }) => formatText(value, true),
  },
  {
    field: 'status',
    headerName: 'Status',
    minWidth: 100,
    tooltipField: 'status',
  },
  {
    field: 'remark',
    headerName: 'Remark',
    minWidth: 180,
    tooltipField: 'remark',
  },
];
