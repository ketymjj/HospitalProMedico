import { Routes } from '@angular/router';

export const MEDICAL_RECORDS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/medical-record-list.component').then((m) => m.MedicalRecordListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/medical-record-form.component').then((m) => m.MedicalRecordFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/medical-record-form.component').then((m) => m.MedicalRecordFormComponent)
  }
];
