import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardDto } from '../../../core/models/dashboard.model';
import { ChartComponent } from '../../../shared/components/chart/chart.component';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/enums';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, ChartComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  data = signal<DashboardDto | null>(null);
  loading = signal(true);

  constructor(private dashboardService: DashboardService, public authService: AuthService) {}

  ngOnInit(): void {
    this.dashboardService.get().subscribe({
      next: (result) => {
        this.data.set(result);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  get canAccessFinance(): boolean {
    return this.authService.hasRole(UserRole.ADMIN, UserRole.FINANCEIRO);
  }

  get canAccessSettings(): boolean {
    return this.authService.hasRole(UserRole.ADMIN);
  }

  readonly appointmentsByMonthLabels = computed(() => this.data()?.appointmentsByMonth.map((p) => p.label) ?? []);
  readonly appointmentsByMonthValues = computed(() => this.data()?.appointmentsByMonth.map((p) => p.value) ?? []);

  readonly hospitalizationsByMonthLabels = computed(() => this.data()?.hospitalizationsByMonth.map((p) => p.label) ?? []);
  readonly hospitalizationsByMonthValues = computed(() => this.data()?.hospitalizationsByMonth.map((p) => p.value) ?? []);

  readonly patientsByAgeRangeLabels = computed(() => this.data()?.patientsByAgeRange.map((p) => p.label) ?? []);
  readonly patientsByAgeRangeValues = computed(() => this.data()?.patientsByAgeRange.map((p) => p.value) ?? []);

  readonly patientsByHealthPlanLabels = computed(() => this.data()?.patientsByHealthPlan.map((p) => p.label) ?? []);
  readonly patientsByHealthPlanValues = computed(() => this.data()?.patientsByHealthPlan.map((p) => p.value) ?? []);

  get stats() {
    const d = this.data();
    if (!d) return [];
    return [
      { label: 'Total de Pacientes', value: d.totalPatients, icon: 'people', color: '#1e50a2' },
      { label: 'Médicos Cadastrados', value: d.totalDoctors, icon: 'medical_services', color: '#00b4c6' },
      { label: 'Consultas Hoje', value: d.appointmentsToday, icon: 'event_available', color: '#1fa971' },
      { label: 'Pacientes Internados', value: d.hospitalizedPatients, icon: 'local_hospital', color: '#f2a900' },
      { label: 'Cirurgias Agendadas', value: d.scheduledSurgeries, icon: 'healing', color: '#6b5ce7' },
      { label: 'Pagamentos Pendentes', value: d.pendingPayments, icon: 'payment', color: '#e04848' }
    ];
  }
}
