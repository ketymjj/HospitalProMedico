import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { AppointmentDto, CreateAppointment, UpdateAppointment } from '../models/appointment.model';
import { buildHttpParams } from './http-params.util';
import { AppointmentStatus } from '../models/enums';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private readonly baseUrl = `${environment.apiUrl}/appointments`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { date?: string; doctorId?: number; patientId?: number; status?: string }): Observable<PagedResult<AppointmentDto>> {
    return this.http.get<PagedResult<AppointmentDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<AppointmentDto> {
    return this.http.get<AppointmentDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateAppointment): Observable<AppointmentDto> {
    return this.http.post<AppointmentDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateAppointment): Observable<AppointmentDto> {
    return this.http.put<AppointmentDto>(`${this.baseUrl}/${id}`, dto);
  }

  updateStatus(id: number, status: AppointmentStatus): Observable<AppointmentDto> {
    return this.http.patch<AppointmentDto>(`${this.baseUrl}/${id}/status`, { status });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
