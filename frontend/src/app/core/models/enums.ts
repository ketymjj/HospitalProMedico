export enum UserRole {
  ADMIN = 'ADMIN',
  MEDICO = 'MEDICO',
  RECEPCAO = 'RECEPCAO',
  FINANCEIRO = 'FINANCEIRO'
}

export enum Gender {
  MASCULINO = 'MASCULINO',
  FEMININO = 'FEMININO',
  OUTRO = 'OUTRO'
}

export enum BloodType {
  A_POSITIVO = 'A_POSITIVO',
  A_NEGATIVO = 'A_NEGATIVO',
  B_POSITIVO = 'B_POSITIVO',
  B_NEGATIVO = 'B_NEGATIVO',
  AB_POSITIVO = 'AB_POSITIVO',
  AB_NEGATIVO = 'AB_NEGATIVO',
  O_POSITIVO = 'O_POSITIVO',
  O_NEGATIVO = 'O_NEGATIVO'
}

export enum RegisterStatus {
  ATIVO = 'ATIVO',
  INATIVO = 'INATIVO'
}

export enum Specialty {
  CARDIOLOGIA = 'CARDIOLOGIA',
  CLINICA_GERAL = 'CLINICA_GERAL',
  PEDIATRIA = 'PEDIATRIA',
  ORTOPEDIA = 'ORTOPEDIA',
  NEUROLOGIA = 'NEUROLOGIA',
  GINECOLOGIA = 'GINECOLOGIA',
  DERMATOLOGIA = 'DERMATOLOGIA',
  OFTALMOLOGIA = 'OFTALMOLOGIA',
  CIRURGIA_GERAL = 'CIRURGIA_GERAL'
}

export enum AppointmentType {
  PRIMEIRA_CONSULTA = 'PRIMEIRA_CONSULTA',
  RETORNO = 'RETORNO',
  URGENCIA = 'URGENCIA',
  EXAME = 'EXAME'
}

export enum AppointmentStatus {
  AGENDADA = 'AGENDADA',
  CONFIRMADA = 'CONFIRMADA',
  REALIZADA = 'REALIZADA',
  CANCELADA = 'CANCELADA'
}

export enum HospitalizationType {
  CLINICA = 'CLINICA',
  CIRURGICA = 'CIRURGICA',
  UTI = 'UTI',
  OBSERVACAO = 'OBSERVACAO'
}

export enum HospitalizationStatus {
  INTERNADO = 'INTERNADO',
  ALTA = 'ALTA',
  CANCELADO = 'CANCELADO'
}

export enum SurgeryStatus {
  AGENDADA = 'AGENDADA',
  EM_PREPARACAO = 'EM_PREPARACAO',
  REALIZADA = 'REALIZADA',
  CANCELADA = 'CANCELADA'
}

export enum HealthPlanType {
  BASICO = 'BASICO',
  INTERMEDIARIO = 'INTERMEDIARIO',
  PREMIUM = 'PREMIUM',
  EMPRESARIAL = 'EMPRESARIAL'
}

export enum PaymentMethod {
  DINHEIRO = 'DINHEIRO',
  PIX = 'PIX',
  CARTAO_CREDITO = 'CARTAO_CREDITO',
  CARTAO_DEBITO = 'CARTAO_DEBITO',
  BOLETO = 'BOLETO',
  CONVENIO = 'CONVENIO'
}

export enum PaymentStatus {
  PENDENTE = 'PENDENTE',
  PAGO = 'PAGO',
  VENCIDO = 'VENCIDO',
  CANCELADO = 'CANCELADO'
}

export enum NotificationType {
  INFO = 'INFO',
  ALERTA = 'ALERTA',
  SUCESSO = 'SUCESSO',
  ERRO = 'ERRO'
}

export const SPECIALTY_LABELS: Record<Specialty, string> = {
  [Specialty.CARDIOLOGIA]: 'Cardiologia',
  [Specialty.CLINICA_GERAL]: 'Clínica Geral',
  [Specialty.PEDIATRIA]: 'Pediatria',
  [Specialty.ORTOPEDIA]: 'Ortopedia',
  [Specialty.NEUROLOGIA]: 'Neurologia',
  [Specialty.GINECOLOGIA]: 'Ginecologia',
  [Specialty.DERMATOLOGIA]: 'Dermatologia',
  [Specialty.OFTALMOLOGIA]: 'Oftalmologia',
  [Specialty.CIRURGIA_GERAL]: 'Cirurgia Geral'
};

export const GENDER_LABELS: Record<Gender, string> = {
  [Gender.MASCULINO]: 'Masculino',
  [Gender.FEMININO]: 'Feminino',
  [Gender.OUTRO]: 'Outro'
};

export const BLOOD_TYPE_LABELS: Record<BloodType, string> = {
  [BloodType.A_POSITIVO]: 'A+',
  [BloodType.A_NEGATIVO]: 'A-',
  [BloodType.B_POSITIVO]: 'B+',
  [BloodType.B_NEGATIVO]: 'B-',
  [BloodType.AB_POSITIVO]: 'AB+',
  [BloodType.AB_NEGATIVO]: 'AB-',
  [BloodType.O_POSITIVO]: 'O+',
  [BloodType.O_NEGATIVO]: 'O-'
};

export const APPOINTMENT_TYPE_LABELS: Record<AppointmentType, string> = {
  [AppointmentType.PRIMEIRA_CONSULTA]: 'Primeira Consulta',
  [AppointmentType.RETORNO]: 'Retorno',
  [AppointmentType.URGENCIA]: 'Urgência',
  [AppointmentType.EXAME]: 'Exame'
};

export const APPOINTMENT_STATUS_LABELS: Record<AppointmentStatus, string> = {
  [AppointmentStatus.AGENDADA]: 'Agendada',
  [AppointmentStatus.CONFIRMADA]: 'Confirmada',
  [AppointmentStatus.REALIZADA]: 'Realizada',
  [AppointmentStatus.CANCELADA]: 'Cancelada'
};

export const HOSPITALIZATION_TYPE_LABELS: Record<HospitalizationType, string> = {
  [HospitalizationType.CLINICA]: 'Clínica',
  [HospitalizationType.CIRURGICA]: 'Cirúrgica',
  [HospitalizationType.UTI]: 'UTI',
  [HospitalizationType.OBSERVACAO]: 'Observação'
};

export const HOSPITALIZATION_STATUS_LABELS: Record<HospitalizationStatus, string> = {
  [HospitalizationStatus.INTERNADO]: 'Internado',
  [HospitalizationStatus.ALTA]: 'Alta',
  [HospitalizationStatus.CANCELADO]: 'Cancelado'
};

export const SURGERY_STATUS_LABELS: Record<SurgeryStatus, string> = {
  [SurgeryStatus.AGENDADA]: 'Agendada',
  [SurgeryStatus.EM_PREPARACAO]: 'Em Preparação',
  [SurgeryStatus.REALIZADA]: 'Realizada',
  [SurgeryStatus.CANCELADA]: 'Cancelada'
};

export const HEALTH_PLAN_TYPE_LABELS: Record<HealthPlanType, string> = {
  [HealthPlanType.BASICO]: 'Básico',
  [HealthPlanType.INTERMEDIARIO]: 'Intermediário',
  [HealthPlanType.PREMIUM]: 'Premium',
  [HealthPlanType.EMPRESARIAL]: 'Empresarial'
};

export const PAYMENT_METHOD_LABELS: Record<PaymentMethod, string> = {
  [PaymentMethod.DINHEIRO]: 'Dinheiro',
  [PaymentMethod.PIX]: 'PIX',
  [PaymentMethod.CARTAO_CREDITO]: 'Cartão de Crédito',
  [PaymentMethod.CARTAO_DEBITO]: 'Cartão de Débito',
  [PaymentMethod.BOLETO]: 'Boleto',
  [PaymentMethod.CONVENIO]: 'Convênio'
};

export const PAYMENT_STATUS_LABELS: Record<PaymentStatus, string> = {
  [PaymentStatus.PENDENTE]: 'Pendente',
  [PaymentStatus.PAGO]: 'Pago',
  [PaymentStatus.VENCIDO]: 'Vencido',
  [PaymentStatus.CANCELADO]: 'Cancelado'
};

export const REGISTER_STATUS_LABELS: Record<RegisterStatus, string> = {
  [RegisterStatus.ATIVO]: 'Ativo',
  [RegisterStatus.INATIVO]: 'Inativo'
};

export const ROLE_LABELS: Record<UserRole, string> = {
  [UserRole.ADMIN]: 'Administrador',
  [UserRole.MEDICO]: 'Médico',
  [UserRole.RECEPCAO]: 'Recepção',
  [UserRole.FINANCEIRO]: 'Financeiro'
};
