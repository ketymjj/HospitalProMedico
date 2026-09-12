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

import { MedicalRecordService } from '../../../core/services/medical-record.service';
import { PatientService } from '../../../core/services/patient.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { ToastService } from '../../../core/services/toast.service';
import { PatientListItem } from '../../../core/models/patient.model';
import { DoctorDto } from '../../../core/models/doctor.model';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-medical-record-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule, MatDatepickerModule, PageHeaderComponent
  ],
  templateUrl: './medical-record-form.component.html'
})
export class MedicalRecordFormComponent implements OnInit {
  private fb = inject(FormBuilder);

  recordId: number | null = null;
  saving = signal(false);
  patients = signal<PatientListItem[]>([]);
  doctors = signal<DoctorDto[]>([]);

  form = this.fb.group({
    patientId: [null as number | null, [Validators.required]],
    doctorId: [null as number | null, [Validators.required]],
    date: [new Date(), [Validators.required]],
    chiefComplaint: ['', [Validators.required]],
    history: ['', [Validators.required]],
    diagnosis: ['', [Validators.required]],
    prescription: ['', [Validators.required]],
    exams: [''],
    notes: ['']
  });

  get isEditMode(): boolean {
    return this.recordId !== null;
  }

  constructor(
    private medicalRecordService: MedicalRecordService,
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
      this.recordId = Number(idParam);
      this.medicalRecordService.getById(this.recordId).subscribe((r) => {
        this.form.patchValue({ ...r, date: new Date(r.date) });
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
      chiefComplaint: raw.chiefComplaint!,
      history: raw.history!,
      diagnosis: raw.diagnosis!,
      prescription: raw.prescription!,
      exams: raw.exams,
      notes: raw.notes
    };

    const request = this.isEditMode
      ? this.medicalRecordService.update(this.recordId!, payload)
      : this.medicalRecordService.create(payload);

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Prontuário atualizado com sucesso.' : 'Prontuário registrado com sucesso.');
        this.router.navigate(['/medical-records']);
      },
      error: () => this.saving.set(false)
    });
  }
}
