import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router, provideRouter } from '@angular/router';
import { LogisticsMenuComponent } from './logistics-menu.component';

describe('LogisticsMenuComponent', () => {
  let component: LogisticsMenuComponent;
  let fixture: ComponentFixture<LogisticsMenuComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LogisticsMenuComponent],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(LogisticsMenuComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create and display Page 1 (LOGISTICS PAPER ROLL MENU) initially', () => {
    expect(component).toBeTruthy();
    expect(component.pageIndex()).toBe(0);

    const titleElement = fixture.nativeElement.querySelector('.page-title');
    expect(titleElement?.textContent?.trim()).toBe('LOGISTICS PAPER ROLL MENU');

    const menuCards = fixture.nativeElement.querySelectorAll('.menu-card');
    expect(menuCards.length).toBe(8);
  });

  it('should render highlighted CONFIRM buttons on Page 1', () => {
    const highlightedCards = fixture.nativeElement.querySelectorAll('.menu-card.highlighted');
    expect(highlightedCards.length).toBe(2);

    const texts = Array.from(highlightedCards).map((card: any) =>
      card.textContent?.replace(/\s+/g, ' ').trim(),
    );
    expect(texts).toContain('Paper Roll Used CONFIRM');
    expect(texts).toContain('Paper Roll Export CONFIRM');
  });

  it('should switch to Page 2 (LOGISTICS FINISHED GOODS MENU) when clicking Next', () => {
    const nextBtn = fixture.nativeElement.querySelector('.nav-btn-next') as HTMLButtonElement;
    nextBtn.click();
    fixture.detectChanges();

    expect(component.pageIndex()).toBe(1);
    const titleElement = fixture.nativeElement.querySelector('.page-title');
    expect(titleElement?.textContent?.trim()).toBe('LOGISTICS FINISHED GOODS MENU');

    const menuCards = fixture.nativeElement.querySelectorAll('.menu-card');
    expect(menuCards.length).toBe(8);
  });

  it('should switch back to Page 1 when clicking Go Back from Page 2', () => {
    component.pageIndex.set(1);
    fixture.detectChanges();

    const backBtn = fixture.nativeElement.querySelector('.nav-btn-back') as HTMLButtonElement;
    backBtn.click();
    fixture.detectChanges();

    expect(component.pageIndex()).toBe(0);
    const titleElement = fixture.nativeElement.querySelector('.page-title');
    expect(titleElement?.textContent?.trim()).toBe('LOGISTICS PAPER ROLL MENU');
  });

  it('should navigate to /home when clicking Go Back from Page 1', () => {
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    const backBtn = fixture.nativeElement.querySelector('.nav-btn-back') as HTMLButtonElement;
    backBtn.click();

    expect(navigateSpy).toHaveBeenCalledWith('/home');
  });

  it('should hide Next on the last page', () => {
    expect(fixture.nativeElement.querySelector('.nav-btn-next')).toBeTruthy();

    component.pageIndex.set(1);
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('.nav-btn-next')).toBeNull();
    component.onNextPage();
    expect(component.pageIndex()).toBe(1);
  });

  it('should navigate if a menu card has an assigned route', () => {
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);

    const stockCard = fixture.nativeElement.querySelector(
      '[data-action="stock"]',
    ) as HTMLButtonElement;
    expect(stockCard).toBeTruthy();
    stockCard.click();

    expect(navigateSpy).toHaveBeenCalledWith('/logistic/current-stock');
  });
});
