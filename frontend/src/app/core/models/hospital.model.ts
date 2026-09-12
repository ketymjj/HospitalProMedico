export interface HospitalDto {
  id: number;
  name: string;
  cnpj: string;
  phone: string;
  email: string;
  address: string;
  logoUrl?: string | null;
}

export interface UpdateHospital {
  name: string;
  cnpj: string;
  phone: string;
  email: string;
  address: string;
  logoUrl?: string | null;
}
