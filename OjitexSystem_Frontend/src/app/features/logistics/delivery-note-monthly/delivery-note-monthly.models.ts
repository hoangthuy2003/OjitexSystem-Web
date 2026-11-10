export interface DeliveryMonthlyFilters {
  deliverySlip: string;
  customerCode: string;
  productCode: string;
  fromDate: string;
  toDate: string;
}

export interface DeliveryMonthlyItem {
  key: number;
  status: string | null;
  lineNo: number | null;
  deliverySlip: string;
  receiveNo: number | null;
  supplierCode: string | null;
  customerCode: number | null;
  customerName: string | null;
  productCode: number | null;
  productName: string | null;
  quantity: number | null;
  deliveryDate: string | null;
  customerPo: string | null;
  remark: string | null;
  otherNote: string | null;
  truckNumber: string | null;
  carType: string | null;
  transportCompany: string | null;
  dateInput: string | null;
  userId: string | null;
  userFirstName: string | null;
  state: string | null;
}

export interface DeliveryMonthlyReport {
  items: DeliveryMonthlyItem[];
}
