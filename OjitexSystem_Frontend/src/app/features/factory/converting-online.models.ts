export interface ConvertingOnlineItem {
  shift: number | null;
  lotNo: number | null;
  orderNo: number | null;
  customerCode: number | null;
  customerName: string | null;
  productCode: number | null;
  productName: string | null;
  planQty: number | null;
  inputQty: number | null;
  outputQty: number | null;
  surplusQty: number | null;
  lossQty: number | null;
  shortestQty: number;
  startTime: string | null;
  finishTime: string | null;
  drivingMinutes: number | null;
  setMinutes: number | null;
  stopMinutes: number | null;
  restMinutes: number | null;
}

export interface ConvertingOnlineReport {
  reportDate: string;
  refreshedAt: string;
  items: ConvertingOnlineItem[];
}
