import { RegisterStatus, Specialty } from './enums';

export interface DoctorDto {
  id: number;
  name: string;
  crm: string;
  specialty: Specialty;
  cpf: string;
  phone: string;
  email: string;
  birthDate: string;
  address: string;
  status: RegisterStatus;
  createdAt: string;
}

export interface DoctorDetail extends DoctorDto {
  appointmentsCount: number;
  surgeriesCount: number;
  hospitalizationsCount: number;
}

export interface CreateDoctor {
  name: string;
  crm: string;
  specialty: Specialty;
  cpf: string;
  phone: string;
  email: string;
  birthDate: string;
  address: string;
}

export interface UpdateDoctor extends CreateDoctor {
  status: RegisterStatus;
}
