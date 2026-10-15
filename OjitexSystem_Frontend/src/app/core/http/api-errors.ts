import { HttpErrorResponse } from '@angular/common/http';
import { TimeoutError } from 'rxjs';

export function apiErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof TimeoutError) {
    return 'The server took too long to respond. Please try again.';
  }
  if (!(error instanceof HttpErrorResponse)) return fallback;
  if (error.status === 0) {
    return 'Unable to connect to the server. Check the backend and try again.';
  }
  const body: unknown = error.error;
  if (typeof body === 'object' && body !== null) {
    const details = body as Record<string, unknown>;
    for (const key of ['message', 'detail', 'title']) {
      const value = details[key];
      if (typeof value === 'string' && value.trim()) return value;
    }
  }
  return fallback;
}
