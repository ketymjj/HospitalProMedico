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

import { DoctorService } from '../../../core/services/doctor.service';
import { ToastService } from '../../../core/services/toast.service';
import { REGISTER_STATUS_LABELS, RegisterStatus, SPECIALTY_LABELS, Specialty } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { PhoneMaskDirective } from '../../../shared/directives/phone-mask.directive';
import { CpfMaskDirective } from '../../../shared/directives/cpf-mask.directive';
import { DateMaskDirective } from '../../../shared/directives/date-mask.directive';
import { formatCpf } from '../../../shared/utils/mask.util';

@Component({
  selector: 'app-doctor-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, MatProgressSpinnerModule, PageHeaderComponent,
    PhoneMaskDirective, CpfMaskDirective, DateMaskDirective
  ],
  templateUrl: './doctor-form.component.html'
})
export class DoctorFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  doctorId: number | null = null;
  saving = signal(false);

  readonly specialtyOptions = Object.values(Specialty);
  readonly specialtyLabels = SPECIALTY_LABELS;
  readonly statusOptions = Object.values(RegisterStatus);
  readonly statusLabels = REGISTER_STATUS_LABELS;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    crm: ['', [Validators.required]],
    specialty: [Specialty.CLINICA_GERAL, [Validators.required]],
    cpf: ['', [Validators.required]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    birthDate: [null as Date | null, [Validators.required]],
    address: ['', [Validators.required]],
    status: [RegisterStatus.ATIVO]
  });

  get isEditMode(): boolean {
    return this.doctorId !== null;
  }

  constructor(
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.doctorId = Number(idParam);
      this.doctorService.getById(this.doctorId).subscribe((doctor) => {
        this.form.patchValue({ ...doctor, cpf: formatCpf(doctor.cpf), birthDate: new Date(doctor.birthDate) });
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
      ...raw,
      birthDate: (raw.birthDate as Date).toISOString(),
      cpf: raw.cpf!.replace(/\D/g, ''),
      name: raw.name!,
      crm: raw.crm!,
      specialty: raw.specialty!,
      phone: raw.phone!,
      email: raw.email!,
      address: raw.address!,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.doctorService.update(this.doctorId!, payload)
      : this.doctorService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Médico atualizado com sucesso.' : 'Médico cadastrado com sucesso.');
        this.router.navigate(['/doctors']);
      },
      error: () => this.saving.set(false)
    });
  }
}
