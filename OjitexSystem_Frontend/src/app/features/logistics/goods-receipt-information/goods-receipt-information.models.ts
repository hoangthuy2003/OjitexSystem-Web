import { GoodsReceiptItem } from '../goods-receipt-search/goods-receipt-search.models';

export interface GoodsReceiptInformationFilters {
  dateReceipt: string;
  receiveNo: string;
  productCode: string;
}

export interface GoodsReceiptInformationReport {
  items: GoodsReceiptItem[];
}
