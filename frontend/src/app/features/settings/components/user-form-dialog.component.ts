import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { UserService } from '../../../core/services/user.service';
import { ToastService } from '../../../core/services/toast.service';
import { User } from '../../../core/models/auth.model';
import { ROLE_LABELS, UserRole } from '../../../core/models/enums';

export interface UserFormDialogData {
  user: User | null;
}

@Component({
  selector: 'app-user-form-dialog',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatSlideToggleModule
  ],
  templateUrl: './user-form-dialog.component.html'
})
export class UserFormDialogComponent {
  private fb = inject(FormBuilder);
  data = inject<UserFormDialogData>(MAT_DIALOG_DATA);

  saving = signal(false);
  isEditMode = !!this.data.user;

  readonly roleOptions = Object.values(UserRole);
  readonly roleLabels = ROLE_LABELS;

  form = this.fb.group({
    name: [this.data.user?.name || '', [Validators.required]],
    email: [this.data.user?.email || '', [Validators.required, Validators.email]],
    role: [this.data.user?.role || UserRole.RECEPCAO, [Validators.required]],
    isActive: [this.data.user?.isActive ?? true],
    password: ['', this.isEditMode ? [] : [Validators.required, Validators.minLength(6)]]
  });

  constructor(
    private userService: UserService,
    private toast: ToastService,
    public dialogRef: MatDialogRef<UserFormDialogComponent>
  ) {}

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.form.getRawValue();

    const request = this.isEditMode
      ? this.userService.update(this.data.user!.id, {
          name: raw.name!,
          email: raw.email!,
          role: raw.role!,
          isActive: raw.isActive!,
          password: raw.password || null
        })
      : this.userService.create({
          name: raw.name!,
          email: raw.email!,
          role: raw.role!,
          password: raw.password!
        });

    request.subscribe({
      next: () => {
        this.toast.success(this.isEditMode ? 'Usuário atualizado com sucesso.' : 'Usuário cadastrado com sucesso.');
        this.dialogRef.close(true);
      },
      error: () => this.saving.set(false)
    });
  }
}
