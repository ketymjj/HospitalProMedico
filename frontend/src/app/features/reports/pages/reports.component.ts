import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import * as XLSX from 'xlsx';

import { ReportService } from '../../../core/services/report.service';
import { ReportsSummary } from '../../../core/models/report.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import {
  APPOINTMENT_STATUS_LABELS,
  HOSPITALIZATION_STATUS_LABELS,
  PAYMENT_STATUS_LABELS,
  REGISTER_STATUS_LABELS,
  SURGERY_STATUS_LABELS
} from '../../../core/models/enums';

type ReportTab = 'patients' | 'appointments' | 'hospitalizations' | 'surgeries' | 'payments' | 'healthPlans';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule, FormsModule, MatFormFieldModule, MatInputModule, MatDatepickerModule,
    MatButtonModule, MatIconModule, MatTabsModule, PageHeaderComponent, EmptyStateComponent
  ],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss'
})
export class ReportsComponent implements OnInit {
  startDate: Date | null = null;
  endDate: Date | null = null;
  data = signal<ReportsSummary | null>(null);
  activeTab: ReportTab = 'patients';

  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly appointmentStatusLabels = APPOINTMENT_STATUS_LABELS;
  readonly hospitalizationStatusLabels = HOSPITALIZATION_STATUS_LABELS;
  readonly surgeryStatusLabels = SURGERY_STATUS_LABELS;
  readonly paymentStatusLabels = PAYMENT_STATUS_LABELS;

  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.reportService
      .get({
        startDate: this.startDate ? this.startDate.toISOString() : null,
        endDate: this.endDate ? this.endDate.toISOString() : null
      })
      .subscribe((result) => this.data.set(result));
  }

  onTabChange(index: number): void {
    const tabs: ReportTab[] = ['patients', 'appointments', 'hospitalizations', 'surgeries', 'payments', 'healthPlans'];
    this.activeTab = tabs[index];
  }

  private currentTableData(): { headers: string[]; rows: (string | number)[][]; title: string } {
    const d = this.data();
    if (!d) return { headers: [], rows: [], title: '' };

    switch (this.activeTab) {
      case 'patients':
        return {
          title: 'Relatório de Pacientes',
          headers: ['Nome', 'CPF', 'Telefone', 'Plano', 'Status'],
          rows: d.patients.map((p) => [p.name, p.cpf, p.phone, p.healthPlanName || 'Particular', this.statusLabels[p.status]])
        };
      case 'appointments':
        return {
          title: 'Relatório de Consultas',
          headers: ['Data', 'Horário', 'Paciente', 'Médico', 'Status'],
          rows: d.appointments.map((a) => [
            new Date(a.date).toLocaleDateString('pt-BR'),
            a.time,
            a.patientName,
            a.doctorName,
            this.appointmentStatusLabels[a.status]
          ])
        };
      case 'hospitalizations':
        return {
          title: 'Relatório de Internações',
          headers: ['Paciente', 'Médico', 'Entrada', 'Quarto/Leito', 'Status'],
          rows: d.hospitalizations.map((h) => [
            h.patientName,
            h.doctorName,
            new Date(h.admissionDate).toLocaleDateString('pt-BR'),
            `${h.room}/${h.bed}`,
            this.hospitalizationStatusLabels[h.status]
          ])
        };
      case 'surgeries':
        return {
          title: 'Relatório de Cirurgias',
          headers: ['Paciente', 'Médico', 'Tipo', 'Data', 'Status'],
          rows: d.surgeries.map((s) => [
            s.patientName,
            s.doctorName,
            s.type,
            new Date(s.date).toLocaleDateString('pt-BR'),
            this.surgeryStatusLabels[s.status]
          ])
        };
      case 'payments':
        return {
          title: 'Relatório de Pagamentos',
          headers: ['Paciente', 'Descrição', 'Valor', 'Vencimento', 'Status'],
          rows: d.payments.map((p) => [
            p.patientName,
            p.description,
            p.amount.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }),
            new Date(p.dueDate).toLocaleDateString('pt-BR'),
            this.paymentStatusLabels[p.status]
          ])
        };
      case 'healthPlans':
        return {
          title: 'Relatório de Planos de Saúde',
          headers: ['Nome', 'Código', 'Pacientes', 'Status'],
          rows: d.healthPlans.map((h) => [h.name, h.code, h.patientsCount, this.statusLabels[h.status]])
        };
    }
  }

  exportPdf(): void {
    const { headers, rows, title } = this.currentTableData();
    const doc = new jsPDF();
    doc.setFontSize(14);
    doc.text('Hospital ProMédico', 14, 15);
    doc.setFontSize(11);
    doc.text(title, 14, 23);
    autoTable(doc, { head: [headers], body: rows as any, startY: 28 });
    doc.save(`${title.toLowerCase().replace(/\s+/g, '-')}.pdf`);
  }

  exportExcel(): void {
    const { headers, rows, title } = this.currentTableData();
    const worksheet = XLSX.utils.aoa_to_sheet([headers, ...rows]);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, title.substring(0, 30));
    XLSX.writeFile(workbook, `${title.toLowerCase().replace(/\s+/g, '-')}.xlsx`);
  }

  printReport(): void {
    window.print();
  }
}
