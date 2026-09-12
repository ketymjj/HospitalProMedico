import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { UserRole } from './core/models/enums';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/pages/login.component').then((m) => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/pages/register.component').then((m) => m.RegisterComponent)
  },
  {
    path: '',
    loadComponent: () => import('./layout/shell/shell.component').then((m) => m.ShellComponent),
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/pages/dashboard.component').then((m) => m.DashboardComponent)
      },
      {
        path: 'patients',
        loadChildren: () => import('./features/patients/patients.routes').then((m) => m.PATIENTS_ROUTES)
      },
      {
        path: 'doctors',
        loadChildren: () => import('./features/doctors/doctors.routes').then((m) => m.DOCTORS_ROUTES)
      },
      {
        path: 'appointments',
        loadChildren: () => import('./features/appointments/appointments.routes').then((m) => m.APPOINTMENTS_ROUTES)
      },
      {
        path: 'hospitalizations',
        loadChildren: () =>
          import('./features/hospitalizations/hospitalizations.routes').then((m) => m.HOSPITALIZATIONS_ROUTES)
      },
      {
        path: 'surgeries',
        loadChildren: () => import('./features/surgeries/surgeries.routes').then((m) => m.SURGERIES_ROUTES)
      },
      {
        path: 'health-plans',
        loadChildren: () => import('./features/health-plans/health-plans.routes').then((m) => m.HEALTH_PLANS_ROUTES)
      },
      {
        path: 'payments',
        canActivate: [roleGuard([UserRole.ADMIN, UserRole.FINANCEIRO])],
        loadChildren: () => import('./features/payments/payments.routes').then((m) => m.PAYMENTS_ROUTES)
      },
      {
        path: 'medical-records',
        loadChildren: () =>
          import('./features/medical-records/medical-records.routes').then((m) => m.MEDICAL_RECORDS_ROUTES)
      },
      {
        path: 'reports',
        loadComponent: () => import('./features/reports/pages/reports.component').then((m) => m.ReportsComponent)
      },
      {
        path: 'settings',
        canActivate: [roleGuard([UserRole.ADMIN])],
        loadChildren: () => import('./features/settings/settings.routes').then((m) => m.SETTINGS_ROUTES)
      }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
