import { SurgeryStatus } from './enums';

export interface SurgeryDto {
  id: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  type: string;
  date: string;
  time: string;
  room: string;
  estimatedDurationMinutes: number;
  status: SurgeryStatus;
  notes?: string | null;
}

export interface CreateSurgery {
  patientId: number;
  doctorId: number;
  type: string;
  date: string;
  time: string;
  room: string;
  estimatedDurationMinutes: number;
  notes?: string | null;
}

export interface UpdateSurgery extends CreateSurgery {
  status: SurgeryStatus;
}
