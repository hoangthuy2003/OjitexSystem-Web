import { ConvertingOnlineItem } from '../converting-online.models';

export interface ConvertingHistoryFilters {
  date: string;
  process: string;
  productCode: string;
}

export interface ConvertingHistoryItem extends ConvertingOnlineItem {
  date: string | null;
  machineName: string | null;
}

export interface ConvertingHistoryReport {
  reportDate: string | null;
  refreshedAt: string;
  items: ConvertingHistoryItem[];
}
