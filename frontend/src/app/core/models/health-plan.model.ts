import { HealthPlanType, RegisterStatus } from './enums';

export interface HealthPlanDto {
  id: number;
  name: string;
  code: string;
  cnpj: string;
  phone: string;
  email: string;
  address: string;
  type: HealthPlanType;
  status: RegisterStatus;
  patientsCount: number;
}

export interface CreateHealthPlan {
  name: string;
  code: string;
  cnpj: string;
  phone: string;
  email: string;
  address: string;
  type: HealthPlanType;
}

export interface UpdateHealthPlan extends CreateHealthPlan {
  status: RegisterStatus;
}
