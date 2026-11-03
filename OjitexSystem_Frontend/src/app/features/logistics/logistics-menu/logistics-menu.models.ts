export interface LogisticsMenuItem {
  title: string;
  lines: string[];
  isHighlighted?: boolean;
  route?: string;
  actionKey?: string;
}

export interface LogisticsMenuPageData {
  id: 'paper-roll' | 'finished-goods';
  title: string;
  items: (LogisticsMenuItem | null)[];
}

export const PAPER_ROLL_MENU_ITEMS: (LogisticsMenuItem | null)[] = [
  {
    title: 'Paper Roll Import Search',
    lines: ['Paper Roll Import', 'Search'],
    actionKey: 'import-search',
  },
  {
    title: 'Paper Roll Used Search',
    lines: ['Paper Roll Used', 'Search'],
    actionKey: 'used-search',
  },
  {
    title: 'Paper Roll Export Search',
    lines: ['Paper Roll Export', 'Search'],
    actionKey: 'export-search',
  },

  {
    title: 'Paper Roll Stock',
    lines: ['Paper Roll Stock'],
    route: '/logistic/current-stock',
    actionKey: 'stock',
  },
  {
    title: 'Paper Roll Used CONFIRM',
    lines: ['Paper Roll Used', 'CONFIRM'],
    isHighlighted: true,
    actionKey: 'used-confirm',
  },
  {
    title: 'Paper Roll Export CONFIRM',
    lines: ['Paper Roll Export', 'CONFIRM'],
    isHighlighted: true,
    actionKey: 'export-confirm',
  },

  {
    title: 'Paper Roll Used Information',
    lines: ['Paper Roll Used', 'Information'],
    actionKey: 'used-info',
  },
  {
    title: 'Paper Roll Export Information',
    lines: ['Paper Roll Export', 'Information'],
    actionKey: 'export-info',
  },
];

export const FINISHED_GOODS_MENU_ITEMS: (LogisticsMenuItem | null)[] = [
  {
    title: 'Delivery Note Information',
    lines: ['Delivery Note', 'Information'],
    actionKey: 'delivery-note-info',
  },

  {
    title: 'Delivery Note Search',
    lines: ['Delivery Note', 'Search'],
    actionKey: 'delivery-note-search',
  },
  {
    title: 'Goods Receipt Search',
    lines: ['Goods Receipt', 'Search'],
    actionKey: 'goods-receipt-search',
  },
  {
    title: 'Delivery Note Monthly',
    lines: ['Delivery Note', 'Monthly'],
    actionKey: 'delivery-note-monthly',
  },

  {
    title: 'Goods Receipt Information',
    lines: ['Goods Receipt', 'Information'],
    actionKey: 'goods-receipt-info',
  },

  {
    title: 'Delivery Plan Search',
    lines: ['Delivery Plan', 'Search'],
    actionKey: 'delivery-plan-search',
  },
  {
    title: 'Delivery Plan Monthly',
    lines: ['Delivery Plan', 'Monthly'],
    actionKey: 'delivery-plan-monthly',
  },
  {
    title: 'Delivery Note Status',
    lines: ['Delivery Note', 'Status'],
    actionKey: 'delivery-note-status',
  },
];

export const LOGISTICS_PAGES: LogisticsMenuPageData[] = [
  {
    id: 'paper-roll',
    title: 'LOGISTICS PAPER ROLL MENU',
    items: PAPER_ROLL_MENU_ITEMS,
  },
  {
    id: 'finished-goods',
    title: 'LOGISTICS FINISHED GOODS MENU',
    items: FINISHED_GOODS_MENU_ITEMS,
  },
];
