import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AlertService } from '../../../../core/services/alert.service';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { ApiError } from '../../../../core/models/api-error.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly alertService = inject(AlertService);

  isLoading = false;
  errorMessage = '';

  loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  onSubmit(): void {
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    this.authService.login(this.loginForm.getRawValue())
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: response => {
          if (!response.success) {
            this.errorMessage = response.message || 'Invalid email or password.';
            return;
          }

          this.alertService.success('Logged in successfully.');
          this.router.navigate(['/dashboard']);
        },
        error: (error: ApiError) => {
          this.errorMessage = error.message || 'Invalid email or password.';
        }
      });
  }
}
