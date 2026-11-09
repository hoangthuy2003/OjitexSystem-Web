export interface GoodsReceiptFilters {
  dateReceipt: string;
  receiveNo: string;
}

export interface GoodsReceiptItem {
  key: number;
  dateReceipt: string | null;
  receiveNo: number | null;
  customerCode: number | null;
  customerName: string | null;
  productCode: number | null;
  productName: string | null;
  quantity: number | null;
  productionSection: string | null;
  note: string | null;
  dateInput: string | null;
  userFirstName: string | null;
  dateChanged: string | null;
  userChangedFirstName: string | null;
}

export interface GoodsReceiptSearchResult {
  items: GoodsReceiptItem[];
}
