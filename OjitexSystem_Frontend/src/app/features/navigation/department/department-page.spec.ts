import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, convertToParamMap, provideRouter } from '@angular/router';
import { DepartmentPage } from './department-page';

describe('Factory menu', () => {
  it('opens Corrugator Online and keeps planned functions disabled', () => {
    TestBed.configureTestingModule({
      imports: [DepartmentPage],
      providers: [
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: convertToParamMap({ departmentId: 'factory' }) } },
        },
      ],
    });
    const fixture = TestBed.createComponent(DepartmentPage);
    fixture.detectChanges();
    const buttons = Array.from(
      fixture.nativeElement.querySelectorAll('.report-card') as NodeListOf<HTMLButtonElement>,
    );
    expect(buttons).toHaveLength(9);
    expect(buttons[0].disabled).toBe(false);
    expect(buttons.slice(1).every((button) => button.disabled)).toBe(true);
    const navigate = vi.spyOn(TestBed.inject(Router), 'navigateByUrl').mockResolvedValue(true);
    buttons[0].click();
    expect(navigate).toHaveBeenCalledWith('/factory/corrugator-online');
  });
});
