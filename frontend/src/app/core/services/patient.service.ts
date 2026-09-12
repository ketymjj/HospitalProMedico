import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreatePatient, PatientDetail, PatientDto, PatientListItem, UpdatePatient } from '../models/patient.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly baseUrl = `${environment.apiUrl}/patients`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { status?: string; healthPlanId?: number }): Observable<PagedResult<PatientListItem>> {
    return this.http.get<PagedResult<PatientListItem>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<PatientDto> {
    return this.http.get<PatientDto>(`${this.baseUrl}/${id}`);
  }

  getDetails(id: number): Observable<PatientDetail> {
    return this.http.get<PatientDetail>(`${this.baseUrl}/${id}/details`);
  }

  create(dto: CreatePatient): Observable<PatientDto> {
    return this.http.post<PatientDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdatePatient): Observable<PatientDto> {
    return this.http.put<PatientDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
