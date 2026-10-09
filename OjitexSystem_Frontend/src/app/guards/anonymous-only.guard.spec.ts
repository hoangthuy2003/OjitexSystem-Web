import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { anonymousOnlyGuard } from './anonymous-only.guard';

describe('anonymousOnlyGuard', () => {
  it('redirects authenticated users to home', () => {
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { accessToken: 'test-token' } },
        {
          provide: Router,
          useValue: { parseUrl: vi.fn((url: string) => ({ url })) },
        },
      ],
    });

    expect(TestBed.runInInjectionContext(() => anonymousOnlyGuard({} as never, {} as never)))
      .toEqual({ url: '/home' });
  });

  it('allows unauthenticated users to visit login', () => {
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { accessToken: null } },
        { provide: Router, useValue: { parseUrl: vi.fn() } },
      ],
    });

    expect(TestBed.runInInjectionContext(() => anonymousOnlyGuard({} as never, {} as never)))
      .toBe(true);
  });
});
