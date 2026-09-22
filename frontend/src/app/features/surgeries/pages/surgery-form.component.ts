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

import { SurgeryService } from '../../../core/services/surgery.service';
import { PatientService } from '../../../core/services/patient.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { ToastService } from '../../../core/services/toast.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { DoctorDto } from '../../../core/models/doctor.model';
import { SURGERY_STATUS_LABELS, SurgeryStatus } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { DateMaskDirective } from '../../../shared/directives/date-mask.directive';

@Component({
  selector: 'app-surgery-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, PageHeaderComponent,
    DateMaskDirective
  ],
  templateUrl: './surgery-form.component.html'
})
export class SurgeryFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  surgeryId: number | null = null;
  saving = signal(false);
  patients = signal<PatientListItem[]>([]);
  doctors = signal<DoctorDto[]>([]);

  readonly statusOptions = Object.values(SurgeryStatus);
  readonly statusLabels = SURGERY_STATUS_LABELS;

  form = this.fb.group({
    patientId: [null as number | null, [Validators.required]],
    doctorId: [null as number | null, [Validators.required]],
    type: ['', [Validators.required]],
    date: [null as Date | null, [Validators.required]],
    time: ['08:00', [Validators.required]],
    room: ['', [Validators.required]],
    estimatedDurationMinutes: [60, [Validators.required, Validators.min(1)]],
    notes: [''],
    status: [SurgeryStatus.AGENDADA]
  });

  get isEditMode(): boolean {
    return this.surgeryId !== null;
  }

  constructor(
    private surgeryService: SurgeryService,
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
      this.surgeryId = Number(idParam);
      this.surgeryService.getById(this.surgeryId).subscribe((s) => {
        this.form.patchValue({ ...s, date: new Date(s.date) });
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
      type: raw.type!,
      date: (raw.date as Date).toISOString(),
      time: raw.time!,
      room: raw.room!,
      estimatedDurationMinutes: raw.estimatedDurationMinutes!,
      notes: raw.notes,
      status: raw.status!
    };

    const request = this.isEditMode
      ? this.surgeryService.update(this.surgeryId!, payload)
      : this.surgeryService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Cirurgia atualizada com sucesso.' : 'Cirurgia agendada com sucesso.');
        this.router.navigate(['/surgeries']);
      },
      error: () => this.saving.set(false)
    });
  }
}
