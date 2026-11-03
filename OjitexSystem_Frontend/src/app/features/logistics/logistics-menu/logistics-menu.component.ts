import { Component, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { LOGISTICS_PAGES, LogisticsMenuItem, LogisticsMenuPageData } from './logistics-menu.models';

@Component({
  selector: 'app-logistics-menu',
  standalone: true,
  templateUrl: './logistics-menu.component.html',
  styleUrl: './logistics-menu.component.scss',
})
export class LogisticsMenuComponent {
  private readonly router = inject(Router);

  readonly pages = LOGISTICS_PAGES;
  readonly pageIndex = signal<number>(0);

  readonly currentPage = computed<LogisticsMenuPageData>(() => this.pages[this.pageIndex()]);

  readonly hasNextPage = computed<boolean>(() => this.pageIndex() < this.pages.length - 1);

  onNextPage(): void {
    if (this.hasNextPage()) {
      this.pageIndex.set(this.pageIndex() + 1);
    }
  }

  onGoBack(): void {
    if (this.pageIndex() > 0) {
      this.pageIndex.set(this.pageIndex() - 1);
    } else {
      void this.router.navigateByUrl('/home');
    }
  }

  onMenuItemClick(item: LogisticsMenuItem | null): void {
    if (!item) {
      return;
    }
    // Reserved for future feature implementation: "code phần nào tôi sẽ nói sau"
    if (item.route) {
      void this.router.navigateByUrl(item.route);
    }
  }
}
