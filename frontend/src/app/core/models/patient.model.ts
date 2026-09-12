import { BloodType, Gender, RegisterStatus } from './enums';
import { AppointmentDto } from './appointment.model';
import { HospitalizationDto } from './hospitalization.model';
import { SurgeryDto } from './surgery.model';
import { MedicalRecordDto } from './medical-record.model';
import { PaymentDto } from './payment.model';

export interface PatientListItem {
  id: number;
  name: string;
  cpf: string;
  phone: string;
  healthPlanName?: string | null;
  birthDate: string;
  status: RegisterStatus;
}

export interface PatientDto {
  id: number;
  name: string;
  cpf: string;
  birthDate: string;
  gender: Gender;
  phone: string;
  email: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  susCardNumber?: string | null;
  healthPlanId?: number | null;
  healthPlanName?: string | null;
  healthPlanCardNumber?: string | null;
  bloodType?: BloodType | null;
  allergies?: string | null;
  notes?: string | null;
  createdAt: string;
  status: RegisterStatus;
}

export interface PatientDetail extends PatientDto {
  appointments: AppointmentDto[];
  hospitalizations: HospitalizationDto[];
  surgeries: SurgeryDto[];
  medicalRecords: MedicalRecordDto[];
  payments: PaymentDto[];
}

export interface CreatePatient {
  name: string;
  cpf: string;
  birthDate: string;
  gender: Gender;
  phone: string;
  email: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  susCardNumber?: string | null;
  healthPlanId?: number | null;
  healthPlanCardNumber?: string | null;
  bloodType?: BloodType | null;
  allergies?: string | null;
  notes?: string | null;
}

export interface UpdatePatient extends CreatePatient {
  status: RegisterStatus;
}
