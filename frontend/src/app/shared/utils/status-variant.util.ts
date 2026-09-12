import { BadgeVariant } from '../components/status-badge/status-badge.component';
import {
  AppointmentStatus,
  HospitalizationStatus,
  PaymentStatus,
  RegisterStatus,
  SurgeryStatus
} from '../../core/models/enums';

export function registerStatusVariant(status: RegisterStatus): BadgeVariant {
  return status === RegisterStatus.ATIVO ? 'success' : 'neutral';
}

export function appointmentStatusVariant(status: AppointmentStatus): BadgeVariant {
  switch (status) {
    case AppointmentStatus.AGENDADA: return 'info';
    case AppointmentStatus.CONFIRMADA: return 'success';
    case AppointmentStatus.REALIZADA: return 'neutral';
    case AppointmentStatus.CANCELADA: return 'danger';
    default: return 'neutral';
  }
}

export function hospitalizationStatusVariant(status: HospitalizationStatus): BadgeVariant {
  switch (status) {
    case HospitalizationStatus.INTERNADO: return 'warning';
    case HospitalizationStatus.ALTA: return 'success';
    case HospitalizationStatus.CANCELADO: return 'danger';
    default: return 'neutral';
  }
}

export function surgeryStatusVariant(status: SurgeryStatus): BadgeVariant {
  switch (status) {
    case SurgeryStatus.AGENDADA: return 'info';
    case SurgeryStatus.EM_PREPARACAO: return 'warning';
    case SurgeryStatus.REALIZADA: return 'success';
    case SurgeryStatus.CANCELADA: return 'danger';
    default: return 'neutral';
  }
}

export function paymentStatusVariant(status: PaymentStatus): BadgeVariant {
  switch (status) {
    case PaymentStatus.PAGO: return 'success';
    case PaymentStatus.PENDENTE: return 'warning';
    case PaymentStatus.VENCIDO: return 'danger';
    case PaymentStatus.CANCELADO: return 'neutral';
    default: return 'neutral';
  }
}
