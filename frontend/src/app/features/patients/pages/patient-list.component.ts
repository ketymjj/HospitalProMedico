import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subject, debounceTime } from 'rxjs';

import { PatientService } from '../../../core/services/patient.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { RegisterStatus, REGISTER_STATUS_LABELS } from '../../../core/models/enums';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { registerStatusVariant } from '../../../shared/utils/status-variant.util';
import { labelOf } from '../../../shared/utils/label.util';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/enums';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatProgressBarModule,
    StatusBadgeComponent,
    EmptyStateComponent,
    PageHeaderComponent
  ],
  providers: [DatePipe],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent implements OnInit {
  displayedColumns = ['name', 'cpf', 'phone', 'healthPlan', 'birthDate', 'status', 'actions'];
  items = signal<PatientListItem[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';
  statusFilter = '';

  readonly statusOptions = Object.values(RegisterStatus);
  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private patientService: PatientService,
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

  get canDelete(): boolean {
    return this.authService.hasRole(UserRole.ADMIN);
  }

  ngOnInit(): void {
    this.load();
  }

  onSearchChange(value: string): void {
    this.search = value;
    this.searchSubject.next(value);
  }

  onStatusChange(): void {
    this.pageNumber = 1;
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.patientService
      .getAll({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        search: this.search,
        status: this.statusFilter || undefined
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

  statusVariant(status: RegisterStatus) {
    return registerStatusVariant(status);
  }

  remove(patient: PatientListItem): void {
    this.confirmDialog
      .confirm({
        title: 'Excluir paciente',
        message: `Tem certeza que deseja excluir o paciente "${patient.name}"? Esta ação não pode ser desfeita.`,
        confirmLabel: 'Excluir',
        danger: true
      })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.patientService.delete(patient.id).subscribe({
          next: () => {
            this.toast.success('Paciente excluído com sucesso.');
            this.load();
          }
        });
      });
  }
}
