export type StockReportKind = 'current' | 'last' | 'monthly';

export interface StockReportItem {
  id: number | null;
  reportDate: string | null;
  productCode: number | null;
  openingStock: number | null;
  warehouseNg: number | null;
  disposal: number | null;
  repair: number | null;
  production: number | null;
  delivery: number | null;
  closingStock: number | null;
}

export interface StockReport {
  workingDayFrom: string | null;
  workingDayTo: string | null;
  items: StockReportItem[];
}

export interface StockReportHistoryItem {
  id: number;
  receiveNo: number | null;
  productCode: number | null;
  section: string | null;
  inputStock: number | null;
  outputStock: number | null;
  currentStock: number | null;
  remark: string | null;
  workDate: string | null;
  dateInput: string | null;
  userId: string | null;
  userFirstName: string | null;
}

export interface StockReportHistory {
  productCode: number;
  productName: string | null;
  customerName: string | null;
  workingDayFrom: string;
  workingDayTo: string;
  items: StockReportHistoryItem[];
}
