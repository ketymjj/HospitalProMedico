import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';

import { PaymentService } from '../../../core/services/payment.service';
import { PatientService } from '../../../core/services/patient.service';
import { ToastService } from '../../../core/services/toast.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { PAYMENT_METHOD_LABELS, PAYMENT_STATUS_LABELS, PaymentMethod, PaymentStatus } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, PageHeaderComponent
  ],
  templateUrl: './payment-form.component.html'
})
export class PaymentFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  paymentId: number | null = null;
  saving = signal(false);
  patients = signal<PatientListItem[]>([]);

  readonly methodOptions = Object.values(PaymentMethod);
  readonly methodLabels = PAYMENT_METHOD_LABELS;
  readonly statusOptions = Object.values(PaymentStatus);
  readonly statusLabels = PAYMENT_STATUS_LABELS;

  form = this.fb.group({
    patientId: [null as number | null, [Validators.required]],
    description: ['', [Validators.required]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    dueDate: [new Date(), [Validators.required]],
    paymentDate: [null as Date | null],
    method: [PaymentMethod.PIX, [Validators.required]],
    status: [PaymentStatus.PENDENTE]
  });

  get isEditMode(): boolean {
    return this.paymentId !== null;
  }

  constructor(
    private paymentService: PaymentService,
    private patientService: PatientService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.patientService.getAll({ pageNumber: 1, pageSize: 200 }).subscribe((r) => this.patients.set(r.items));

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.paymentId = Number(idParam);
      this.paymentService.getById(this.paymentId).subscribe((p) => {
        this.form.patchValue({
          ...p,
          dueDate: new Date(p.dueDate),
          paymentDate: p.paymentDate ? new Date(p.paymentDate) : null
        });
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.form.getRawValue();
    const payload = {
      patientId: raw.patientId!,
      description: raw.description!,
      amount: raw.amount!,
      dueDate: (raw.dueDate as Date).toISOString(),
      paymentDate: raw.paymentDate ? (raw.paymentDate as Date).toISOString() : null,
      method: raw.method!,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.paymentService.update(this.paymentId!, payload)
      : this.paymentService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Pagamento atualizado com sucesso.' : 'Pagamento registrado com sucesso.');
        this.router.navigate(['/payments']);
      },
      error: () => this.saving.set(false)
    });
  }
}
