import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateHospitalization, HospitalizationDto, UpdateHospitalization } from '../models/hospitalization.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class HospitalizationService {
  private readonly baseUrl = `${environment.apiUrl}/hospitalizations`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { status?: string }): Observable<PagedResult<HospitalizationDto>> {
    return this.http.get<PagedResult<HospitalizationDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getCurrent(): Observable<HospitalizationDto[]> {
    return this.http.get<HospitalizationDto[]>(`${this.baseUrl}/current`);
  }

  getById(id: number): Observable<HospitalizationDto> {
    return this.http.get<HospitalizationDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateHospitalization): Observable<HospitalizationDto> {
    return this.http.post<HospitalizationDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateHospitalization): Observable<HospitalizationDto> {
    return this.http.put<HospitalizationDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
