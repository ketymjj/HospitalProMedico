import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateMedicalRecord, MedicalRecordDto, UpdateMedicalRecord } from '../models/medical-record.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class MedicalRecordService {
  private readonly baseUrl = `${environment.apiUrl}/medical-records`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { patientId?: number }): Observable<PagedResult<MedicalRecordDto>> {
    return this.http.get<PagedResult<MedicalRecordDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<MedicalRecordDto> {
    return this.http.get<MedicalRecordDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateMedicalRecord): Observable<MedicalRecordDto> {
    return this.http.post<MedicalRecordDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateMedicalRecord): Observable<MedicalRecordDto> {
    return this.http.put<MedicalRecordDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
