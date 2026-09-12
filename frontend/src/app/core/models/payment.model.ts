import { PaymentMethod, PaymentStatus } from './enums';

export interface PaymentDto {
  id: number;
  patientId: number;
  patientName: string;
  description: string;
  amount: number;
  dueDate: string;
  paymentDate?: string | null;
  method: PaymentMethod;
  status: PaymentStatus;
}

export interface CreatePayment {
  patientId: number;
  description: string;
  amount: number;
  dueDate: string;
  method: PaymentMethod;
}

export interface UpdatePayment extends CreatePayment {
  paymentDate?: string | null;
  status: PaymentStatus;
}

export interface MonthlyRevenue {
  month: string;
  total: number;
}

export interface FinancialSummary {
  totalReceived: number;
  totalPending: number;
  totalOverdue: number;
  monthlyRevenue: MonthlyRevenue[];
}
