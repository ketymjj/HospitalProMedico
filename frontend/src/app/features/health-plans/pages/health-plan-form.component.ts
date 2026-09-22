import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { HealthPlanService } from '../../../core/services/health-plan.service';
import { ToastService } from '../../../core/services/toast.service';
import { HEALTH_PLAN_TYPE_LABELS, HealthPlanType, REGISTER_STATUS_LABELS, RegisterStatus } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { CnpjMaskDirective } from '../../../shared/directives/cnpj-mask.directive';
import { PhoneMaskDirective } from '../../../shared/directives/phone-mask.directive';

@Component({
  selector: 'app-health-plan-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, PageHeaderComponent,
    CnpjMaskDirective, PhoneMaskDirective
  ],
  templateUrl: './health-plan-form.component.html'
})
export class HealthPlanFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  planId: number | null = null;
  saving = signal(false);

  readonly typeOptions = Object.values(HealthPlanType);
  readonly typeLabels = HEALTH_PLAN_TYPE_LABELS;
  readonly statusOptions = Object.values(RegisterStatus);
  readonly statusLabels = REGISTER_STATUS_LABELS;

  form = this.fb.group({
    name: ['', [Validators.required]],
    code: ['', [Validators.required]],
    cnpj: ['', [Validators.required]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    address: ['', [Validators.required]],
    type: [HealthPlanType.BASICO, [Validators.required]],
    status: [RegisterStatus.ATIVO]
  });

  get isEditMode(): boolean {
    return this.planId !== null;
  }

  constructor(
    private healthPlanService: HealthPlanService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.planId = Number(idParam);
      this.healthPlanService.getById(this.planId).subscribe((plan) => this.form.patchValue(plan));
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
      name: raw.name!,
      code: raw.code!,
      cnpj: raw.cnpj!,
      phone: raw.phone!,
      email: raw.email!,
      address: raw.address!,
      type: raw.type!,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.healthPlanService.update(this.planId!, payload)
      : this.healthPlanService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Plano atualizado com sucesso.' : 'Plano cadastrado com sucesso.');
        this.router.navigate(['/health-plans']);
      },
      error: () => this.saving.set(false)
    });
  }
}
