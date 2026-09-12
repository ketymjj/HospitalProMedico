import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreatePayment, FinancialSummary, PaymentDto, UpdatePayment } from '../models/payment.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private readonly baseUrl = `${environment.apiUrl}/payments`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams & { status?: string }): Observable<PagedResult<PaymentDto>> {
    return this.http.get<PagedResult<PaymentDto>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getSummary(): Observable<FinancialSummary> {
    return this.http.get<FinancialSummary>(`${this.baseUrl}/summary`);
  }

  getById(id: number): Observable<PaymentDto> {
    return this.http.get<PaymentDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreatePayment): Observable<PaymentDto> {
    return this.http.post<PaymentDto>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdatePayment): Observable<PaymentDto> {
    return this.http.put<PaymentDto>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
