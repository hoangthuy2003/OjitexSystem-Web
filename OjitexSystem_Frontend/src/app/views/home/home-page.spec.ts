import { TestBed } from '@angular/core/testing';
import { Router, provideRouter } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HomePage } from './home-page';

describe('HomePage', () => {
  async function createPage(isAdmin: boolean) {
    await TestBed.configureTestingModule({
      imports: [HomePage],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: { isAdmin } },
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(HomePage);
    fixture.detectChanges();
    return fixture;
  }

  it('shows an access-denied dialog to users without the admin role', async () => {
    const fixture = await createPage(false);
    const adminButton = fixture.nativeElement.querySelector(
      'button.department-card',
    ) as HTMLButtonElement;

    adminButton.click();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('[role="alertdialog"]').textContent)
      .toContain('Bạn không có quyền truy cập tính năng này.');
  });

  it('navigates directly to the admin dashboard for administrators', async () => {
    const fixture = await createPage(true);
    const router = TestBed.inject(Router);
    const navigateSpy = vi.spyOn(router, 'navigateByUrl').mockResolvedValue(true);
    const adminButton = fixture.nativeElement.querySelector(
      'button.department-card',
    ) as HTMLButtonElement;

    adminButton.click();

    expect(navigateSpy).toHaveBeenCalledWith('/admin/dashboard');
    expect(fixture.nativeElement.querySelector('[role="alertdialog"]')).toBeNull();
  });
});
