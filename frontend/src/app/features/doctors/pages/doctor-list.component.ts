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
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subject, debounceTime } from 'rxjs';

import { DoctorService } from '../../../core/services/doctor.service';
import { DoctorDto } from '../../../core/models/doctor.model';
import { RegisterStatus, REGISTER_STATUS_LABELS, Specialty, SPECIALTY_LABELS } from '../../../core/models/enums';
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
  selector: 'app-doctor-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatIconModule,
    MatTooltipModule, MatProgressBarModule, StatusBadgeComponent, EmptyStateComponent, PageHeaderComponent
  ],
  templateUrl: './doctor-list.component.html',
  styleUrl: './doctor-list.component.scss'
})
export class DoctorListComponent implements OnInit {
  displayedColumns = ['name', 'crm', 'specialty', 'phone', 'status', 'actions'];
  items = signal<DoctorDto[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';
  specialtyFilter = '';
  statusFilter = '';

  readonly specialtyOptions = Object.values(Specialty);
  readonly specialtyLabels = SPECIALTY_LABELS;
  readonly statusOptions = Object.values(RegisterStatus);
  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private doctorService: DoctorService,
    private confirmDialog: ConfirmDialogService,
    private toast: ToastService,
    public authService: AuthService
  ) {
    this.searchSubject.pipe(debounceTime(400)).subscribe(() => {
      this.pageNumber = 1;
      this.load();
    });
  }

  get isAdmin(): boolean {
    return this.authService.hasRole(UserRole.ADMIN);
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

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.doctorService
      .getAll({
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        search: this.search,
        specialty: this.specialtyFilter || undefined,
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

  remove(doctor: DoctorDto): void {
    this.confirmDialog
      .confirm({
        title: 'Excluir médico',
        message: `Tem certeza que deseja excluir o médico "${doctor.name}"?`,
        confirmLabel: 'Excluir',
        danger: true
      })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.doctorService.delete(doctor.id).subscribe(() => {
          this.toast.success('Médico excluído com sucesso.');
          this.load();
        });
      });
  }
}
