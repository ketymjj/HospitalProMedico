import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

import { DoctorService } from '../../../core/services/doctor.service';
import { DoctorDetail } from '../../../core/models/doctor.model';
import { REGISTER_STATUS_LABELS, SPECIALTY_LABELS } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { registerStatusVariant } from '../../../shared/utils/status-variant.util';

@Component({
  selector: 'app-doctor-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, PageHeaderComponent, StatusBadgeComponent],
  templateUrl: './doctor-detail.component.html',
  styleUrl: './doctor-detail.component.scss'
})
export class DoctorDetailComponent implements OnInit {
  doctor = signal<DoctorDetail | null>(null);
  readonly specialtyLabels = SPECIALTY_LABELS;
  readonly statusLabels = REGISTER_STATUS_LABELS;
  readonly registerStatusVariant = registerStatusVariant;

  constructor(private route: ActivatedRoute, private doctorService: DoctorService) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.doctorService.getById(id).subscribe((doctor) => this.doctor.set(doctor));
  }
}
