import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CurrentStockApi } from './current-stock.api';
import { CurrentStock } from './current-stock.model';

type StockFilter = 'all' | 'available' | 'empty';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnInit {
  private readonly currentStockApi = inject(CurrentStockApi);
  private detailRequestId = 0;

  stockItems: CurrentStock[] = [];
  selectedStock: CurrentStock | null = null;
  loading = true;
  detailLoading = false;
  errorMessage: string | null = null;
  detailError: string | null = null;
  searchTerm = '';
  stockFilter: StockFilter = 'all';

  ngOnInit(): void {
    this.loadStocks();
  }

  get filteredStocks(): CurrentStock[] {
    const query = this.searchTerm.trim();
    return this.stockItems.filter((stock) => {
      const matchesQuery = !query || String(stock.cstProCd).includes(query);
      const matchesFilter =
        this.stockFilter === 'all' ||
        (this.stockFilter === 'available' && stock.cstStock !== null && stock.cstStock > 0) ||
        (this.stockFilter === 'empty' && stock.cstStock !== null && stock.cstStock <= 0);
      return matchesQuery && matchesFilter;
    });
  }

  get totalStock(): number {
    return this.stockItems.reduce((total, stock) => total + (stock.cstStock ?? 0), 0);
  }

  get outOfStockCount(): number {
    return this.stockItems.filter((stock) => stock.cstStock !== null && stock.cstStock <= 0).length;
  }

  loadStocks(): void {
    this.detailRequestId++;
    this.loading = true;
    this.detailLoading = false;
    this.errorMessage = null;
    this.selectedStock = null;
    this.detailError = null;

    this.currentStockApi.getAll().subscribe({
      next: (stocks) => {
        this.stockItems = stocks;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Vui lòng kiểm tra kết nối API rồi thử lại.';
      },
    });
  }

  selectStock(stock: CurrentStock): void {
    const requestId = ++this.detailRequestId;
    this.selectedStock = null;
    this.detailLoading = true;
    this.detailError = null;

    this.currentStockApi.getByProductCode(stock.cstProCd).subscribe({
      next: (result) => {
        if (requestId !== this.detailRequestId) {
          return;
        }
        this.selectedStock = result;
        this.detailLoading = false;
      },
      error: () => {
        if (requestId !== this.detailRequestId) {
          return;
        }
        this.detailLoading = false;
        this.detailError = 'Không thể tải chi tiết mặt hàng này. Vui lòng thử lại.';
      },
    });
  }

  formatNumber(value: number | null): string {
    return value === null
      ? '—'
      : new Intl.NumberFormat('vi-VN', { maximumFractionDigits: 20 }).format(value);
  }

  stockStatusClass(value: number | null): string {
    if (value === null) {
      return 'stock-badge unknown';
    }
    return value > 0 ? 'stock-badge in-stock' : 'stock-badge out-of-stock';
  }

  stockStatusLabel(value: number | null): string {
    if (value === null) {
      return 'Chưa có dữ liệu';
    }
    return value > 0 ? 'Còn hàng' : 'Hết hàng';
  }
}
