export interface DeliveryStatusFilters {
  fromDate: string;
  toDate: string;
}

export interface DeliveryStatusItem {
  key: number;
  customerCode: number | null;
  customerName: string | null;
  deliveryDate: string | null;
  transportSupplier: string | null;
  truckNumber: string | null;
  deliverySlip: string;
  status: string | null;
  remark: string | null;
}

export interface DeliveryStatusReport {
  items: DeliveryStatusItem[];
}
