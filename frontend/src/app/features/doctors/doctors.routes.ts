import { Routes } from '@angular/router';

export const DOCTORS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/doctor-list.component').then((m) => m.DoctorListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/doctor-form.component').then((m) => m.DoctorFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/doctor-form.component').then((m) => m.DoctorFormComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/doctor-detail.component').then((m) => m.DoctorDetailComponent)
  }
];
