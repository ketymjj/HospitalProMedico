import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';

import { HospitalService } from '../../../core/services/hospital.service';
import { UserService } from '../../../core/services/user.service';
import { ToastService } from '../../../core/services/toast.service';
import { ConfirmDialogService } from '../../../core/services/confirm-dialog.service';
import { User } from '../../../core/models/auth.model';
import { AuthService } from '../../../core/services/auth.service';
import { ROLE_LABELS } from '../../../core/models/enums';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { UserFormDialogComponent } from '../components/user-form-dialog.component';
import { labelOf } from '../../../shared/utils/label.util';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatTabsModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatTableModule, MatTooltipModule, PageHeaderComponent, StatusBadgeComponent
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
  private fb = inject(FormBuilder);

  saving = signal(false);
  users = signal<User[]>([]);
  displayedColumns = ['name', 'email', 'role', 'status', 'actions'];
  readonly roleLabels = ROLE_LABELS;
  readonly labelOf = labelOf;

  hospitalForm = this.fb.group({
    name: ['', [Validators.required]],
    cnpj: ['', [Validators.required]],
    phone: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    address: ['', [Validators.required]],
    logoUrl: ['']
  });

  constructor(
    private hospitalService: HospitalService,
    private userService: UserService,
    private toast: ToastService,
    private confirmDialog: ConfirmDialogService,
    private dialog: MatDialog,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.hospitalService.get().subscribe((hospital) => this.hospitalForm.patchValue(hospital));
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll({ pageNumber: 1, pageSize: 100 }).subscribe((result) => this.users.set(result.items));
  }

  saveHospital(): void {
    if (this.hospitalForm.invalid) {
      this.hospitalForm.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.hospitalForm.getRawValue();
    this.hospitalService
      .update({
        name: raw.name!,
        cnpj: raw.cnpj!,
        phone: raw.phone!,
        email: raw.email!,
        address: raw.address!,
        logoUrl: raw.logoUrl
      })
      .subscribe({
        next: () => {
          this.toast.success('Dados do hospital atualizados com sucesso.');
          this.saving.set(false);
        },
        error: () => this.saving.set(false)
      });
  }

  openUserDialog(user: User | null): void {
    const dialogRef = this.dialog.open(UserFormDialogComponent, {
      width: '460px',
      data: { user }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) this.loadUsers();
    });
  }

  removeUser(user: User): void {
    this.confirmDialog
      .confirm({ title: 'Excluir usuário', message: `Deseja excluir o usuário "${user.name}"?`, confirmLabel: 'Excluir', danger: true })
      .subscribe((confirmed) => {
        if (!confirmed) return;
        this.userService.delete(user.id).subscribe(() => {
          this.toast.success('Usuário excluído.');
          this.loadUsers();
        });
      });
  }
}
