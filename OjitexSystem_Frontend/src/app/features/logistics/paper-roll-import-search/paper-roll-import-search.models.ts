export interface PaperRollImportFilters {
  paperLabel: string;
  gradeName: string;
  dateReceived: string;
}

export interface PaperRollImportItem {
  serialNo: string;
  paperLabel: string;
  gradeName: string | null;
  grammage: number | null;
  paperWidth: number | null;
  importKg: number | null;
  paperType: string | null;
  supplierName: string | null;
  containerNo: string | null;
  dateReceived: string | null;
  dateInput: string | null;
  dateEdit: string | null;
  userInput: string | null;
  remark: string | null;
}

export interface PaperRollImportSearchResult {
  items: PaperRollImportItem[];
}
