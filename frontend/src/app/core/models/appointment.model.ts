import { AppointmentStatus, AppointmentType, Specialty } from './enums';

export interface AppointmentDto {
  id: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  specialty: Specialty;
  date: string;
  time: string;
  type: AppointmentType;
  status: AppointmentStatus;
  notes?: string | null;
}

export interface CreateAppointment {
  patientId: number;
  doctorId: number;
  date: string;
  time: string;
  type: AppointmentType;
  notes?: string | null;
}

export interface UpdateAppointment extends CreateAppointment {
  status: AppointmentStatus;
}
