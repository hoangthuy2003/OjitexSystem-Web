import { TestBed } from '@angular/core/testing';
import { LoginPage } from './login-page';

describe('LoginPage', () => {
  async function createPage() {
    await TestBed.configureTestingModule({
      imports: [LoginPage],
    }).compileComponents();

    const fixture = TestBed.createComponent(LoginPage);
    fixture.detectChanges();
    return fixture;
  }

  it('toggles password visibility', async () => {
    const fixture = await createPage();
    const password = fixture.nativeElement.querySelector('#password') as HTMLInputElement;
    const visibilityButton = fixture.nativeElement.querySelector(
      '.visibility-button',
    ) as HTMLButtonElement;

    expect(password.type).toBe('password');
    visibilityButton.click();
    fixture.detectChanges();
    expect(password.type).toBe('text');
  });

  it('shows that authentication is not connected yet', async () => {
    const fixture = await createPage();

    fixture.componentInstance.submitLogin();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('[role="status"]').textContent).toContain(
      'chưa được kết nối',
    );
  });
});
