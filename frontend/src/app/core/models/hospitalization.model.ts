import { HospitalizationStatus, HospitalizationType } from './enums';

export interface HospitalizationDto {
  id: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  admissionDate: string;
  expectedDischargeDate?: string | null;
  dischargeDate?: string | null;
  room: string;
  bed: string;
  type: HospitalizationType;
  diagnosis: string;
  reason: string;
  notes?: string | null;
  status: HospitalizationStatus;
}

export interface CreateHospitalization {
  patientId: number;
  doctorId: number;
  admissionDate: string;
  expectedDischargeDate?: string | null;
  room: string;
  bed: string;
  type: HospitalizationType;
  diagnosis: string;
  reason: string;
  notes?: string | null;
}

export interface UpdateHospitalization extends CreateHospitalization {
  dischargeDate?: string | null;
  status: HospitalizationStatus;
}
