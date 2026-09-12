import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';
import { ApiErrorResponse } from '../models/common.model';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const toast = inject(ToastService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        toast.error('Sua sessão expirou. Faça login novamente.');
        authService.logout();
        router.navigate(['/login']);
      } else if (error.status === 403) {
        toast.error('Você não tem permissão para realizar esta ação.');
      } else if (error.status === 0) {
        toast.error('Não foi possível conectar ao servidor. Verifique sua conexão.');
      } else {
        const body = error.error as ApiErrorResponse | undefined;
        const message = body?.errors?.length ? body.errors.join(' ') : body?.message || 'Ocorreu um erro inesperado.';
        toast.error(message);
      }

      return throwError(() => error);
    })
  );
};
