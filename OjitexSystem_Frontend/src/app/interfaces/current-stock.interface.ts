export interface CurrentStock {
  cstProCd: number;
  cstOpenStock: number | null;
  cstWarehouseNg: number | null;
  cstDisposal: number | null;
  cstRepair: number | null;
  cstProduction: number | null;
  cstDelivery: number | null;
  cstStock: number | null;
}

export interface CurrentStockPage {
  items: CurrentStock[];
  hasMore: boolean;
  page: number;
  pageSize: number;
}

export interface CurrentStockHistoryItem {
  csthId: number;
  csthReceiveNo: number | null;
  csthProCd: number | null;
  csthSection: string | null;
  csthInputStock: number | null;
  csthOutputStock: number | null;
  csthCurrentStock: number | null;
  csthRemark: string | null;
  csthWorkDate: string | null;
  csthDateInput: string | null;
  csthUserId: string | null;
  userFirstName: string | null;
}

export interface CurrentStockHistory {
  productCode: number;
  productName: string | null;
  customerName: string | null;
  workingDayFrom: string | null;
  workingDayTo: string | null;
  items: CurrentStockHistoryItem[];
}
