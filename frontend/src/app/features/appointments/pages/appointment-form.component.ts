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

import { AppointmentService } from '../../../core/services/appointment.service';
import { PatientService } from '../../../core/services/patient.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { ToastService } from '../../../core/services/toast.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { DoctorDto } from '../../../core/models/doctor.model';
import { AppointmentStatus, APPOINTMENT_STATUS_LABELS, APPOINTMENT_TYPE_LABELS, AppointmentType } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, PageHeaderComponent
  ],
  templateUrl: './appointment-form.component.html'
})
export class AppointmentFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  appointmentId: number | null = null;
  saving = signal(false);
  patients = signal<PatientListItem[]>([]);
  doctors = signal<DoctorDto[]>([]);

  readonly typeOptions = Object.values(AppointmentType);
  readonly typeLabels = APPOINTMENT_TYPE_LABELS;
  readonly statusOptions = Object.values(AppointmentStatus);
  readonly statusLabels = APPOINTMENT_STATUS_LABELS;

  form = this.fb.group({
    patientId: [null as number | null, [Validators.required]],
    doctorId: [null as number | null, [Validators.required]],
    date: [null as Date | null, [Validators.required]],
    time: ['08:00', [Validators.required]],
    type: [AppointmentType.PRIMEIRA_CONSULTA, [Validators.required]],
    status: [AppointmentStatus.AGENDADA],
    notes: ['']
  });

  get isEditMode(): boolean {
    return this.appointmentId !== null;
  }

  constructor(
    private appointmentService: AppointmentService,
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
      this.appointmentId = Number(idParam);
      this.appointmentService.getById(this.appointmentId).subscribe((a) => {
        this.form.patchValue({ ...a, date: new Date(a.date) });
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
      date: (raw.date as Date).toISOString(),
      time: raw.time!,
      type: raw.type!,
      status: raw.status!,
      notes: raw.notes
    };

    const request = this.isEditMode
      ? this.appointmentService.update(this.appointmentId!, payload)
      : this.appointmentService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Consulta atualizada com sucesso.' : 'Consulta agendada com sucesso.');
        this.router.navigate(['/appointments']);
      },
      error: () => this.saving.set(false)
    });
  }
}
