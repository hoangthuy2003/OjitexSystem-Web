import { Directive, ElementRef, HostListener, inject } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { CellApiModule, ColumnApiModule, ModuleRegistry, RowApiModule } from 'ag-grid-community';
import { copyReportRange } from './report-clipboard';

ModuleRegistry.registerModules([CellApiModule, ColumnApiModule, RowApiModule]);

@Directive({ selector: '[reportClipboard]' })
export class ReportClipboardDirective {
  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef).nativeElement;
  private readonly grid = inject(AgGridAngular, { self: true, optional: true });
  private allSelected = false;

  @HostListener('pointerdown')
  protected resetSelection(): void {
    this.allSelected = false;
  }

  @HostListener('keydown', ['$event'])
  protected selectAll(event: KeyboardEvent): void {
    if ((event.target as HTMLElement).closest('input, textarea, [contenteditable="true"]')) return;
    if (this.grid && (event.ctrlKey || event.metaKey) && event.key.toLowerCase() === 'a') {
      const range = this.host.ownerDocument.createRange();
      range.selectNodeContents(this.host);
      const selection = this.host.ownerDocument.getSelection();
      selection?.removeAllRanges();
      selection?.addRange(range);
      this.allSelected = true;
      event.preventDefault();
    } else if (!(event.ctrlKey || event.metaKey) || event.key.toLowerCase() !== 'c') {
      this.allSelected = false;
    }
  }

  @HostListener('copy', ['$event'])
  protected copy(event: ClipboardEvent): void {
    if ((event.target as HTMLElement).closest('input, textarea, [contenteditable="true"]')) return;
    copyReportRange(event, this.host, this.grid?.api, this.allSelected);
  }
}
