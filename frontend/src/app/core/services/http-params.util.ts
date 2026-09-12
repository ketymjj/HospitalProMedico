import { HttpParams } from '@angular/common/http';

export function buildHttpParams(params: object): HttpParams {
  let httpParams = new HttpParams();

  Object.entries(params as Record<string, unknown>).forEach(([key, value]) => {
    if (value === null || value === undefined || value === '') return;
    httpParams = httpParams.set(key, String(value));
  });

  return httpParams;
}
