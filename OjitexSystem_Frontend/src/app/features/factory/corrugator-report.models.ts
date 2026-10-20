export interface CorrugatorReportItem {
  planDate: string | null;
  orderNo: number | null;
  targetCut: number | null;
  qualityItem: number | null;
  paperWidth: number | null;
  corruLength: number | null;
  sheetWidth: number | null;
  corruUp: number | null;
  flute: string | null;
  glue: string | null;
  bm: string | null;
  bl: string | null;
  am: string | null;
  al: string | null;
  driveMinutes: number | null;
  stopMinutes: number | null;
  averageSpeed: number | null;
  endTime: string | null;
  corrugatedDate: string | null;
  dateInput: string | null;
}

export interface CorrugatorReport {
  reportDate: string;
  refreshedAt: string;
  items: CorrugatorReportItem[];
}
