import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { Subject, debounceTime } from 'rxjs';

import { AppointmentService } from '../../../core/services/appointment.service';
import { AppointmentDto } from '../../../core/models/appointment.model';
import { AppointmentStatus, APPOINTMENT_STATUS_LABELS, APPOINTMENT_TYPE_LABELS } from '../../../core/models/enums';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { appointmentStatusVariant } from '../../../shared/utils/status-variant.util';
import { labelOf } from '../../../shared/utils/label.util';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/enums';
import { DateMaskDirective } from '../../../shared/directives/date-mask.directive';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatButtonToggleModule, MatIconModule, MatTooltipModule,
    MatProgressBarModule, MatDatepickerModule, StatusBadgeComponent, EmptyStateComponent, PageHeaderComponent,
    DateMaskDirective
  ],
  templateUrl: './appointment-list.component.html',
  styleUrl: './appointment-list.component.scss'
})
export class AppointmentListComponent implements OnInit {
  displayedColumns = ['date', 'time', 'patient', 'doctor', 'type', 'status', 'actions'];
  items = signal<AppointmentDto[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';
  statusFilter = '';
  dateFilter: Date | null = null;
  view: 'list' | 'agenda' = 'list';

  readonly statusOptions = Object.values(AppointmentStatus);
  readonly statusLabels = APPOINTMENT_STATUS_LABELS;
  readonly typeLabels = APPOINTMENT_TYPE_LABELS;
  readonly AppointmentStatus = AppointmentStatus;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private appointmentService: AppointmentService,
    private confirmDialog: ConfirmDialogService,
    private toast: ToastService,
    public authService: AuthService
  ) {
    this.searchSubject.pipe(debounceTime(400)).subscribe(() => {
      this.pageNumber = 1;
      this.load();
    });
  }

  get canManage(): boolean {
    return this.authService.hasRole(UserRole.ADMIN, UserRole.RECEPCAO);
  }

  ngOnInit(): void {
    this.load();
  }

  onSearchChange(value: string): void {
    this.search = value;
    this.searchSubject.next(value);
  }

  onFilterChange(): void {
    this.pageNumber = 1;
    this.load();
  }

  onDateChange(date: Date | null): void {
    this.dateFilter = date;
    this.onFilterChange();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.appointmentService
      .getAll({
        pageNumber: this.pageNumber,
        pageSize: this.view === 'agenda' ? 200 : this.pageSize,
        search: this.search,
        status: this.statusFilter || undefined,
        date: this.dateFilter ? this.dateFilter.toISOString() : undefined
      })
      .subscribe({
        next: (result) => {
          this.items.set(result.items);
          this.totalCount.set(result.totalCount);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  get groupedByDate(): { date: string; items: AppointmentDto[] }[] {
    const groups = new Map<string, AppointmentDto[]>();
    for (const item of this.items()) {
      const key = item.date.substring(0, 10);
      if (!groups.has(key)) groups.set(key, []);
      groups.get(key)!.push(item);
    }
    return Array.from(groups.entries())
      .sort((a, b) => a[0].localeCompare(b[0]))
      .map(([date, items]) => ({ date, items: items.sort((x, y) => x.time.localeCompare(y.time)) }));
  }

  statusVariant(status: AppointmentStatus) {
    return appointmentStatusVariant(status);
  }

  confirmAppointment(a: AppointmentDto): void {
    this.updateStatus(a, AppointmentStatus.CONFIRMADA);
  }

  finishAppointment(a: AppointmentDto): void {
    this.updateStatus(a, AppointmentStatus.REALIZADA);
  }

  cancelAppointment(a: AppointmentDto): void {
    this.confirmDialog
      .confirm({ title: 'Cancelar consulta', message: `Deseja cancelar a consulta de "${a.patientName}"?`, confirmLabel: 'Cancelar consulta', danger: true })
      .subscribe((confirmed) => {
        if (confirmed) this.updateStatus(a, AppointmentStatus.CANCELADA);
      });
  }

  private updateStatus(a: AppointmentDto, status: AppointmentStatus): void {
    this.appointmentService.updateStatus(a.id, status).subscribe(() => {
      this.toast.success('Status da consulta atualizado.');
      this.load();
    });
  }

  remove(a: AppointmentDto): void {
    this.confirmDialog
      .confirm({ title: 'Excluir consulta', message: `Deseja excluir a consulta de "${a.patientName}"?`, confirmLabel: 'Excluir', danger: true })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.appointmentService.delete(a.id).subscribe(() => {
          this.toast.success('Consulta excluída.');
          this.load();
        });
      });
  }
}
