export interface DeliveryPlanMonthlyFilters {
  fromDate: string;
  toDate: string;
  productCode: string;
}

export interface DeliveryPlanMonthlyItem {
  acceptanceNo: number;
  deliveryDate: string | null;
  customerName: string | null;
  receiveNo: number | null;
  productCode: number | null;
  productName: string | null;
  quantity: number | null;
  stock: number | null;
  currentStock: number | null;
  actualLoading: number | null;
  poNo: string | null;
  remark: string | null;
  sqm: number | null;
  totalSqm: number | null;
  flute: string | null;
  lastProcess: string | null;
  dateInput: string | null;
  dateEdit: string | null;
  userName: string | null;
}

export interface DeliveryPlanMonthlyReport {
  items: DeliveryPlanMonthlyItem[];
}
