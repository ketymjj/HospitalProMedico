import { Routes } from '@angular/router';

export const PAYMENTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/payment-list.component').then((m) => m.PaymentListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/payment-form.component').then((m) => m.PaymentFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/payment-form.component').then((m) => m.PaymentFormComponent)
  }
];
