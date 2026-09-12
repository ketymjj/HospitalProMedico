import { Component, OnInit, computed, signal } from '@angular/core';
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

import { PaymentService } from '../../../core/services/payment.service';
import { PaymentDto, FinancialSummary } from '../../../core/models/payment.model';
import { PAYMENT_METHOD_LABELS, PAYMENT_STATUS_LABELS, PaymentStatus } from '../../../core/models/enums';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { ChartComponent } from '../../../shared/components/chart/chart.component';
import { paymentStatusVariant } from '../../../shared/utils/status-variant.util';
import { labelOf } from '../../../shared/utils/label.util';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/enums';

@Component({
  selector: 'app-payment-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule, MatTableModule, MatPaginatorModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatIconModule, MatTooltipModule, MatProgressBarModule,
    StatusBadgeComponent, EmptyStateComponent, PageHeaderComponent, ChartComponent
  ],
  templateUrl: './payment-list.component.html',
  styleUrl: './payment-list.component.scss'
})
export class PaymentListComponent implements OnInit {
  displayedColumns = ['patient', 'description', 'amount', 'dueDate', 'method', 'status', 'actions'];
  items = signal<PaymentDto[]>([]);
  summary = signal<FinancialSummary | null>(null);
  loading = signal(false);
  totalCount = signal(0);
  pageSize = 10;
  pageNumber = 1;
  search = '';
  statusFilter = '';

  readonly statusOptions = Object.values(PaymentStatus);
  readonly statusLabels = PAYMENT_STATUS_LABELS;
  readonly methodLabels = PAYMENT_METHOD_LABELS;
  readonly labelOf = labelOf;

  private searchSubject = new Subject<string>();

  constructor(
    private paymentService: PaymentService,
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

  ngOnInit(): void {
    this.load();
    this.loadSummary();
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
    this.paymentService
      .getAll({ pageNumber: this.pageNumber, pageSize: this.pageSize, search: this.search, status: this.statusFilter || undefined })
      .subscribe({
        next: (result) => {
          this.items.set(result.items);
          this.totalCount.set(result.totalCount);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
  }

  loadSummary(): void {
    this.paymentService.getSummary().subscribe((summary) => this.summary.set(summary));
  }

  readonly monthlyLabels = computed(() => this.summary()?.monthlyRevenue.map((m) => m.month) ?? []);
  readonly monthlyValues = computed(() => this.summary()?.monthlyRevenue.map((m) => m.total) ?? []);

  statusVariant(status: PaymentStatus) {
    return paymentStatusVariant(status);
  }

  remove(item: PaymentDto): void {
    this.confirmDialog
      .confirm({ title: 'Excluir pagamento', message: `Deseja excluir este pagamento de "${item.patientName}"?`, confirmLabel: 'Excluir', danger: true })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.paymentService.delete(item.id).subscribe(() => {
          this.toast.success('Pagamento excluído.');
          this.load();
          this.loadSummary();
        });
      });
  }
}
