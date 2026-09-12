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
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { PatientService } from '../../../core/services/patient.service';
import { HealthPlanService } from '../../../core/services/health-plan.service';
import { ToastService } from '../../../core/services/toast.service';
import { HealthPlanDto } from '../../../core/models/health-plan.model';
import {
  BLOOD_TYPE_LABELS,
  BloodType,
  GENDER_LABELS,
  Gender,
  REGISTER_STATUS_LABELS,
  RegisterStatus
} from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { PhoneMaskDirective } from '../../../shared/directives/phone-mask.directive';
import { CpfMaskDirective } from '../../../shared/directives/cpf-mask.directive';
import { CepMaskDirective } from '../../../shared/directives/cep-mask.directive';
import { DateMaskDirective } from '../../../shared/directives/date-mask.directive';
import { DigitsOnlyDirective } from '../../../shared/directives/digits-only.directive';
import { formatCpf } from '../../../shared/utils/mask.util';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatProgressSpinnerModule,
    PageHeaderComponent,
    PhoneMaskDirective,
    CpfMaskDirective,
    CepMaskDirective,
    DateMaskDirective,
    DigitsOnlyDirective
  ],
  templateUrl: './patient-form.component.html',
  styleUrl: './patient-form.component.scss'
})
export class PatientFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  patientId: number | null = null;
  loading = signal(false);
  saving = signal(false);
  healthPlans = signal<HealthPlanDto[]>([]);

  readonly genderOptions = Object.values(Gender);
  readonly genderLabels = GENDER_LABELS;
  readonly bloodTypeOptions = Object.values(BloodType);
  readonly bloodTypeLabels = BLOOD_TYPE_LABELS;
  readonly statusOptions = Object.values(RegisterStatus);
  readonly statusLabels = REGISTER_STATUS_LABELS;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    cpf: ['', [Validators.required]],
    birthDate: [null as Date | null, [Validators.required]],
    gender: [Gender.MASCULINO, [Validators.required]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    address: ['', [Validators.required]],
    city: ['', [Validators.required]],
    state: ['', [Validators.required, Validators.maxLength(2)]],
    zipCode: ['', [Validators.required]],
    susCardNumber: [''],
    healthPlanId: [null as number | null],
    healthPlanCardNumber: [''],
    bloodType: [null as BloodType | null],
    allergies: [''],
    notes: [''],
    status: [RegisterStatus.ATIVO]
  });

  get isEditMode(): boolean {
    return this.patientId !== null;
  }

  constructor(
    private patientService: PatientService,
    private healthPlanService: HealthPlanService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.healthPlanService.getAll({ pageNumber: 1, pageSize: 100 }).subscribe((result) => {
      this.healthPlans.set(result.items);
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.patientId = Number(idParam);
      this.loadPatient(this.patientId);
    }
  }

  loadPatient(id: number): void {
    this.loading.set(true);
    this.patientService.getById(id).subscribe({
      next: (patient) => {
        this.form.patchValue({
          ...patient,
          cpf: formatCpf(patient.cpf),
          birthDate: new Date(patient.birthDate)
        });
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.form.getRawValue();
    const payload = {
      ...raw,
      birthDate: (raw.birthDate as Date).toISOString(),
      cpf: raw.cpf!.replace(/\D/g, ''),
      gender: raw.gender!,
      name: raw.name!,
      phone: raw.phone!,
      email: raw.email!,
      address: raw.address!,
      city: raw.city!,
      state: raw.state!,
      zipCode: raw.zipCode!,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.patientService.update(this.patientId!, payload)
      : this.patientService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Paciente atualizado com sucesso.' : 'Paciente cadastrado com sucesso.');
        this.router.navigate(['/patients']);
      },
      error: () => this.saving.set(false)
    });
  }
}
