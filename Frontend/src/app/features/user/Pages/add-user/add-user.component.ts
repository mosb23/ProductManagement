import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiError } from '../../../../core/models/api-error.model';
import { AlertService } from '../../../../core/services/alert.service';
import { applyApiFieldErrors, getBackendErrors } from '../../../../core/utils/form-error.util';
import { passwordMatchValidator } from '../../../../core/validators/password-match.validator';
import { passwordStrengthValidator } from '../../../../core/validators/password-strength.validator';
import { PasswordStrengthComponent } from '../../../../shared/components/password-strength/password-strength.component';
import { ClaimLabelPipe } from '../../../../shared/pipes/claim-label-pipe';
import { RoleClaimResponse } from '../../core/models/role-claim-response.model';
import { RoleResponse } from '../../core/models/role-response.model';
import { AddUserService } from '../../core/services/add-user.service';
import { RolesListService } from '../../core/services/roles-list.service';



@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    PasswordStrengthComponent,
    ClaimLabelPipe
  ],
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent implements OnInit {

  private readonly fb = inject(FormBuilder);
  private readonly rolesService = inject(RolesListService);
  private readonly addUserService = inject(AddUserService);
  private readonly alertService = inject(AlertService);
  private readonly router = inject(Router);

  isSaving = false;

  roles: RoleResponse[] = [];
  claims: RoleClaimResponse[] = [];

  form = this.fb.group({
    fullName: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100)
      ]
    ],

    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      '',
      [
        Validators.required,
        Validators.minLength(8),
        passwordStrengthValidator
      ]
    ],

    confirmPassword: [
      '',
      [
        Validators.required
      ]
    ],

    role: [
      '',
      [
        Validators.required
      ]
    ]

  }, {
    validators: passwordMatchValidator(
      'password',
      'confirmPassword'
    )
  });

  ngOnInit(): void {
    this.loadRoles();

    this.form.controls.role.valueChanges.subscribe(roleId => {

      const role = this.roles.find(x => x.name === roleId);

      if (!role) {
        this.claims = [];
        return;
      }

      this.loadClaims(role.id);
    });
  }

  loadRoles(): void {

    this.rolesService
      .getRoles(1, 100)
      .subscribe({
        next: response => {
          this.roles = response.data?.data ?? [];
        }
      });
  }

  loadClaims(roleId: string): void {

    this.rolesService
      .getRoleClaims(roleId)
      .subscribe({
        next: response => {
          this.claims = response.data ?? [];
        }
      });
  }

  onSubmit(): void {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    this.addUserService
      .createUser(this.form.getRawValue() as any)
      .pipe(finalize(() => this.isSaving = false))
      .subscribe({
        next: response => {

          if (!response.success) {
            this.alertService.error(response.message);
            return;
          }

          this.alertService.success('User created successfully.');

          this.router.navigate(['/users']);
        },
        error: (error: ApiError) => {
          applyApiFieldErrors(this.form, error);
          this.alertService.error(error.message || 'Failed to create user.');
        }
      });
  }

  get password() {
    return this.form.controls.password;
  }

  backendErrors(controlName: string): string[] {
    return getBackendErrors(this.form, controlName);
  }
}
