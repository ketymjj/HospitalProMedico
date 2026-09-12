import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { PatientService } from '../../../core/services/patient.service';
import { PatientDetail } from '../../../core/models/patient.model';
import {
  APPOINTMENT_STATUS_LABELS,
  BLOOD_TYPE_LABELS,
  GENDER_LABELS,
  HOSPITALIZATION_STATUS_LABELS,
  PAYMENT_STATUS_LABELS,
  REGISTER_STATUS_LABELS,
  SURGERY_STATUS_LABELS
} from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import {
  appointmentStatusVariant,
  hospitalizationStatusVariant,
  paymentStatusVariant,
  registerStatusVariant,
  surgeryStatusVariant
} from '../../../shared/utils/status-variant.util';

@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTabsModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    PageHeaderComponent,
    StatusBadgeComponent,
    EmptyStateComponent
  ],
  templateUrl: './patient-detail.component.html',
  styleUrl: './patient-detail.component.scss'
})
export class PatientDetailComponent implements OnInit {
  patient = signal<PatientDetail | null>(null);
  loading = signal(true);

  readonly genderLabels = GENDER_LABELS;
  readonly bloodTypeLabels = BLOOD_TYPE_LABELS;
  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly appointmentStatusLabels = APPOINTMENT_STATUS_LABELS;
  readonly hospitalizationStatusLabels = HOSPITALIZATION_STATUS_LABELS;
  readonly surgeryStatusLabels = SURGERY_STATUS_LABELS;
  readonly paymentStatusLabels = PAYMENT_STATUS_LABELS;

  readonly registerStatusVariant = registerStatusVariant;
  readonly appointmentStatusVariant = appointmentStatusVariant;
  readonly hospitalizationStatusVariant = hospitalizationStatusVariant;
  readonly surgeryStatusVariant = surgeryStatusVariant;
  readonly paymentStatusVariant = paymentStatusVariant;

  constructor(private route: ActivatedRoute, private patientService: PatientService) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.patientService.getDetails(id).subscribe({
      next: (data) => {
        this.patient.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}
