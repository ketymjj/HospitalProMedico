import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateSurgery, SurgeryDto, UpdateSurgery } from '../models/surgery.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class SurgeryService {
  private readonly baseUrl = `${environment.apiUrl}/surgeries`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { status?: string }): Observable<PagedResult<SurgeryDto>> {
    return this.http.get<PagedResult<SurgeryDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<SurgeryDto> {
    return this.http.get<SurgeryDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateSurgery): Observable<SurgeryDto> {
    return this.http.post<SurgeryDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateSurgery): Observable<SurgeryDto> {
    return this.http.put<SurgeryDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
