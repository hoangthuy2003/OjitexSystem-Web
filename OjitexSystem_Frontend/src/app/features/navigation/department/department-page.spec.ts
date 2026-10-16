import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, convertToParamMap, provideRouter } from '@angular/router';
import { DepartmentPage } from './department-page';

describe('Factory menu', () => {
  it('opens implemented Online reports and keeps planned functions disabled', () => {
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
    expect(buttons[1].disabled).toBe(false);
    expect(buttons[4].disabled).toBe(false);
    expect(buttons[5].disabled).toBe(false);
    expect(buttons[7].disabled).toBe(false);
    expect(buttons[8].disabled).toBe(false);
    expect(
      buttons
        .filter((_, index) => ![0, 1, 4, 5, 7, 8].includes(index))
        .every((button) => button.disabled),
    ).toBe(true);
    const navigate = vi.spyOn(TestBed.inject(Router), 'navigateByUrl').mockResolvedValue(true);
    buttons[0].click();
    expect(navigate).toHaveBeenCalledWith('/factory/corrugator-online');
    buttons[1].click();
    expect(navigate).toHaveBeenCalledWith('/factory/fdc-online');
    buttons[4].click();
    expect(navigate).toHaveBeenCalledWith('/factory/evol-online');
    buttons[8].click();
    expect(navigate).toHaveBeenCalledWith('/factory/fg-online');
    buttons[5].click();
    expect(navigate).toHaveBeenCalledWith('/factory/fap-online');
    buttons[7].click();
    expect(navigate).toHaveBeenCalledWith('/factory/fps-online');
  });
});
