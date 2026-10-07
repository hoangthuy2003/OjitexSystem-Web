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
