import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateDoctor, DoctorDetail, DoctorDto, UpdateDoctor } from '../models/doctor.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private readonly baseUrl = `${environment.apiUrl}/doctors`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { specialty?: string; status?: string }): Observable<PagedResult<DoctorDto>> {
    return this.http.get<PagedResult<DoctorDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<DoctorDetail> {
    return this.http.get<DoctorDetail>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateDoctor): Observable<DoctorDto> {
    return this.http.post<DoctorDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateDoctor): Observable<DoctorDto> {
    return this.http.put<DoctorDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
