export interface MedicalRecordDto {
  id: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  date: string;
  chiefComplaint: string;
  history: string;
  diagnosis: string;
  prescription: string;
  exams?: string | null;
  notes?: string | null;
}

export interface CreateMedicalRecord {
  patientId: number;
  doctorId: number;
  date: string;
  chiefComplaint: string;
  history: string;
  diagnosis: string;
  prescription: string;
  exams?: string | null;
  notes?: string | null;
}

export interface UpdateMedicalRecord extends CreateMedicalRecord {}
