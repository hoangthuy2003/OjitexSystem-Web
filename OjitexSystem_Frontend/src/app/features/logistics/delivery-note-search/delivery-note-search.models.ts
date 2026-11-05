export interface DeliveryNoteHeader {
  key: number;
  deliverySlip: string;
  customerCode: number | null;
  customerName: string | null;
  transportCompany: string | null;
  truckNumber: string | null;
  carType: string | null;
  deliveryDate: string | null;
  state: string | null;
  supplierCode: string | null;
  note: string | null;
  deliveryAddress: string | null;
  goodsType: string | null;
  dateChange: string | null;
  dateInput: string | null;
  userId: string | null;
  userFirstName: string | null;
}

export interface DeliveryNoteDetail {
  key: number;
  deliverySlip: string | null;
  lineNo: number | null;
  receiveNo: number | null;
  productCode: number | null;
  productName: string | null;
  quantity: number | null;
  customerPo: string | null;
  remark: string | null;
  otherNote: string | null;
}

export interface DeliveryNote {
  header: DeliveryNoteHeader;
  details: DeliveryNoteDetail[];
}
