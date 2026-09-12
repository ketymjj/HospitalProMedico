import { Routes } from '@angular/router';

export const HEALTH_PLANS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/health-plan-list.component').then((m) => m.HealthPlanListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/health-plan-form.component').then((m) => m.HealthPlanFormComponent)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/health-plan-form.component').then((m) => m.HealthPlanFormComponent)
  }
];
