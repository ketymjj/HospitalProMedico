import { Routes } from '@angular/router';

export const SURGERIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/surgery-list.component').then((m) => m.SurgeryListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/surgery-form.component').then((m) => m.SurgeryFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/surgery-form.component').then((m) => m.SurgeryFormComponent)
  }
];
