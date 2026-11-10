export interface DeliveryPlanFilters {
  deliveryDate: string;
  receiveNo: string;
}

export interface DeliveryPlanItem {
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
  acceptanceNote: string | null;
  dateInput: string | null;
  userFirstName: string | null;
  dateEdit: string | null;
  userEditFirstName: string | null;
}

export interface DeliveryPlanSearchResult {
  items: DeliveryPlanItem[];
}
