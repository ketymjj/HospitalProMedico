import { PatientListItem } from './patient.model';
import { AppointmentDto } from './appointment.model';
import { HospitalizationDto } from './hospitalization.model';
import { SurgeryDto } from './surgery.model';
import { PaymentDto } from './payment.model';
import { HealthPlanDto } from './health-plan.model';

export interface ReportFilter {
  startDate?: string | null;
  endDate?: string | null;
}

export interface ReportsSummary {
  patients: PatientListItem[];
  appointments: AppointmentDto[];
  hospitalizations: HospitalizationDto[];
  surgeries: SurgeryDto[];
  payments: PaymentDto[];
  totalRevenue: number;
  healthPlans: HealthPlanDto[];
}
