import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HospitalDto, UpdateHospital } from '../models/hospital.model';

@Injectable({ providedIn: 'root' })
export class HospitalService {
  private readonly baseUrl = `${environment.apiUrl}/settings/hospital`;

  constructor(private http: HttpClient) {}

  get(): Observable<HospitalDto> {
    return this.http.get<HospitalDto>(this.baseUrl);
  }

  update(dto: UpdateHospital): Observable<HospitalDto> {
    return this.http.put<HospitalDto>(this.baseUrl, dto);
  }
}
