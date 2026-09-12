import { Routes } from '@angular/router';

export const HOSPITALIZATIONS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/hospitalization-list.component').then((m) => m.HospitalizationListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/hospitalization-form.component').then((m) => m.HospitalizationFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/hospitalization-form.component').then((m) => m.HospitalizationFormComponent)
  }
];
