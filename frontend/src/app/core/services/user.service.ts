import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult, PaginationParams } from '../models/common.model';
import { CreateUser, UpdateUser, User } from '../models/auth.model';
import { buildHttpParams } from './http-params.util';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAll(params: PaginationParams): Observable<PagedResult<User>> {
    return this.http.get<PagedResult<User>>(this.baseUrl, { params: buildHttpParams(params) });
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateUser): Observable<User> {
    return this.http.post<User>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateUser): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
