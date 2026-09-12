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

import { HospitalizationService } from '../../../core/services/hospitalization.service';
import { PatientService } from '../../../core/services/patient.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { ToastService } from '../../../core/services/toast.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { DoctorDto } from '../../../core/models/doctor.model';
import {
  HOSPITALIZATION_STATUS_LABELS,
  HOSPITALIZATION_TYPE_LABELS,
  HospitalizationStatus,
  HospitalizationType
} from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-hospitalization-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, PageHeaderComponent
  ],
  templateUrl: './hospitalization-form.component.html'
})
export class HospitalizationFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  hospitalizationId: number | null = null;
  saving = signal(false);
  patients = signal<PatientListItem[]>([]);
  doctors = signal<DoctorDto[]>([]);

  readonly typeOptions = Object.values(HospitalizationType);
  readonly typeLabels = HOSPITALIZATION_TYPE_LABELS;
  readonly statusOptions = Object.values(HospitalizationStatus);
  readonly statusLabels = HOSPITALIZATION_STATUS_LABELS;

  form = this.fb.group({
    patientId: [null as number | null, [Validators.required]],
    doctorId: [null as number | null, [Validators.required]],
    admissionDate: [new Date(), [Validators.required]],
    expectedDischargeDate: [null as Date | null],
    dischargeDate: [null as Date | null],
    room: ['', [Validators.required]],
    bed: ['', [Validators.required]],
    type: [HospitalizationType.CLINICA, [Validators.required]],
    diagnosis: ['', [Validators.required]],
    reason: ['', [Validators.required]],
    notes: [''],
    status: [HospitalizationStatus.INTERNADO]
  });

  get isEditMode(): boolean {
    return this.hospitalizationId !== null;
  }

  constructor(
    private hospitalizationService: HospitalizationService,
    private patientService: PatientService,
    private doctorService: DoctorService,
    private route: ActivatedRoute,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.patientService.getAll({ pageNumber: 1, pageSize: 200 }).subscribe((r) => this.patients.set(r.items));
    this.doctorService.getAll({ pageNumber: 1, pageSize: 200, status: 'ATIVO' }).subscribe((r) => this.doctors.set(r.items));

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.hospitalizationId = Number(idParam);
      this.hospitalizationService.getById(this.hospitalizationId).subscribe((h) => {
        this.form.patchValue({
          ...h,
          admissionDate: new Date(h.admissionDate),
          expectedDischargeDate: h.expectedDischargeDate ? new Date(h.expectedDischargeDate) : null,
          dischargeDate: h.dischargeDate ? new Date(h.dischargeDate) : null
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
      doctorId: raw.doctorId!,
      admissionDate: (raw.admissionDate as Date).toISOString(),
      expectedDischargeDate: raw.expectedDischargeDate ? (raw.expectedDischargeDate as Date).toISOString() : null,
      dischargeDate: raw.dischargeDate ? (raw.dischargeDate as Date).toISOString() : null,
      room: raw.room!,
      bed: raw.bed!,
      type: raw.type!,
      diagnosis: raw.diagnosis!,
      reason: raw.reason!,
      notes: raw.notes,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.hospitalizationService.update(this.hospitalizationId!, payload)
      : this.hospitalizationService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Internação atualizada com sucesso.' : 'Internação registrada com sucesso.');
        this.router.navigate(['/hospitalizations']);
      },
      error: () => this.saving.set(false)
    });
  }
}
