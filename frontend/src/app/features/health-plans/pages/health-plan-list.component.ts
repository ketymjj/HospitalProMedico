import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subject, debounceTime } from 'rxjs';

import { HealthPlanService } from '../../../core/services/health-plan.service';
import { HealthPlanDto } from '../../../core/models/health-plan.model';
import { HEALTH_PLAN_TYPE_LABELS, REGISTER_STATUS_LABELS, RegisterStatus } from '../../../core/models/enums';
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
  selector: 'app-health-plan-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatFormFieldModule,
    MatInputModule, MatButtonModule, MatIconModule, MatTooltipModule, MatProgressBarModule,
    StatusBadgeComponent, EmptyStateComponent, PageHeaderComponent
  ],
  templateUrl: './health-plan-list.component.html',
  styleUrl: './health-plan-list.component.scss'
})
export class HealthPlanListComponent implements OnInit {
  displayedColumns = ['name', 'code', 'type', 'phone', 'patients', 'status', 'actions'];
  items = signal<HealthPlanDto[]>([]);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';

  readonly typeLabels = HEALTH_PLAN_TYPE_LABELS;
  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private healthPlanService: HealthPlanService,
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
    return this.authService.hasRole(UserRole.ADMIN, UserRole.FINANCEIRO);
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

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.healthPlanService.getAll({ pageNumber: this.pageNumber, pageSize: this.pageSize, search: this.search }).subscribe({
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

  remove(item: HealthPlanDto): void {
    this.confirmDialog
      .confirm({ title: 'Excluir plano de saúde', message: `Deseja excluir o plano "${item.name}"?`, confirmLabel: 'Excluir', danger: true })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.healthPlanService.delete(item.id).subscribe(() => {
          this.toast.success('Plano de saúde excluído.');
          this.load();
        });
      });
  }
}
