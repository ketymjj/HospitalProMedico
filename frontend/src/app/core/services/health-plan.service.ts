import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateHealthPlan, HealthPlanDto, UpdateHealthPlan } from '../models/health-plan.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class HealthPlanService {
  private readonly baseUrl = `${environment.apiUrl}/health-plans`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams): Observable<PagedResult<HealthPlanDto>> {
    return this.http.get<PagedResult<HealthPlanDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<HealthPlanDto> {
    return this.http.get<HealthPlanDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateHealthPlan): Observable<HealthPlanDto> {
    return this.http.post<HealthPlanDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateHealthPlan): Observable<HealthPlanDto> {
    return this.http.put<HealthPlanDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
