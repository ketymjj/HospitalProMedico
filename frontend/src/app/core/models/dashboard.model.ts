import { ChartPoint } from './common.model';

export interface DashboardDto {
  totalPatients: number;
  totalDoctors: number;
  appointmentsToday: number;
  hospitalizedPatients: number;
  scheduledSurgeries: number;
  pendingPayments: number;
  appointmentsByMonth: ChartPoint[];
  hospitalizationsByMonth: ChartPoint[];
  patientsByAgeRange: ChartPoint[];
  patientsByHealthPlan: ChartPoint[];
}
