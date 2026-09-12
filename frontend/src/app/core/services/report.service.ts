import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ReportFilter, ReportsSummary } from '../models/report.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class ReportService {
  private readonly baseUrl = `${environment.apiUrl}/reports`;

  constructor(private http: HttpClient) {}

  get(filter: ReportFilter): Observable<ReportsSummary> {
    return this.http.get<ReportsSummary>(this.baseUrl, { params: buildHttpParams(filter) });
  }
}
