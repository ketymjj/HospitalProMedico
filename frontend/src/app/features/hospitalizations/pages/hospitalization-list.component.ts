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
import { Subject, debounceTime } from 'rxjs';

import { HospitalizationService } from '../../../core/services/hospitalization.service';
import { HospitalizationDto } from '../../../core/models/hospitalization.model';
import { HOSPITALIZATION_STATUS_LABELS, HOSPITALIZATION_TYPE_LABELS, HospitalizationStatus } from '../../../core/models/enums';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { hospitalizationStatusVariant } from '../../../shared/utils/status-variant.util';
import { labelOf } from '../../../shared/utils/label.util';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/enums';

@Component({
  selector: 'app-hospitalization-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatButtonToggleModule, MatIconModule, MatTooltipModule,
    MatProgressBarModule, StatusBadgeComponent, EmptyStateComponent, PageHeaderComponent
  ],
  templateUrl: './hospitalization-list.component.html',
  styleUrl: './hospitalization-list.component.scss'
})
export class HospitalizationListComponent implements OnInit {
  displayedColumns = ['patient', 'doctor', 'admission', 'room', 'type', 'status', 'actions'];
  items = signal<HospitalizationDto[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';
  statusFilter = '';
  onlyCurrent = false;

  readonly statusOptions = Object.values(HospitalizationStatus);
  readonly statusLabels = HOSPITALIZATION_STATUS_LABELS;
  readonly typeLabels = HOSPITALIZATION_TYPE_LABELS;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private hospitalizationService: HospitalizationService,
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
    return this.authService.hasRole(UserRole.ADMIN, UserRole.RECEPCAO, UserRole.MEDICO);
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

  toggleCurrent(): void {
    this.onlyCurrent = !this.onlyCurrent;
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

    if (this.onlyCurrent) {
      this.hospitalizationService.getCurrent().subscribe({
        next: (result) => {
          this.items.set(result);
          this.totalCount.set(result.length);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
      return;
    }

    this.hospitalizationService
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

  statusVariant(status: HospitalizationStatus) {
    return hospitalizationStatusVariant(status);
  }

  remove(item: HospitalizationDto): void {
    this.confirmDialog
      .confirm({ title: 'Excluir internação', message: `Deseja excluir esta internação de "${item.patientName}"?`, confirmLabel: 'Excluir', danger: true })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.hospitalizationService.delete(item.id).subscribe(() => {
          this.toast.success('Internação excluída.');
          this.load();
        });
      });
  }
}
