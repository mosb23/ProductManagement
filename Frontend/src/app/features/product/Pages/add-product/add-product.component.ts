import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiError } from '../../../../core/models/api-error.model';
import { AlertService } from '../../../../core/services/alert.service';
import { applyApiFieldErrors, getBackendErrors } from '../../../../core/utils/form-error.util';
import { ProductService } from '../../Core/services/product.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.scss'
})
export class AddProductComponent {
  private readonly fb = inject(FormBuilder);
  private readonly productService = inject(ProductService);
  private readonly alertService = inject(AlertService);
  private readonly router = inject(Router);
  private readonly location = inject(Location);

  isSaving = false;

  productForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
    description: ['', [Validators.maxLength(1000)]],
    price: [0, [Validators.required, Validators.min(0.01)]],
    quantity: [0, [Validators.required, Validators.min(0)]]
  });

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    const formValue = this.productForm.getRawValue();

    this.productService.createProduct({
      name: formValue.name.trim(),
      description: formValue.description?.trim() || null,
      price: Number(formValue.price),
      quantity: Number(formValue.quantity)
    })
    .pipe(finalize(() => this.isSaving = false))
    .subscribe({
      next: response => {
        if (!response.success) {
          this.alertService.error(response.message || 'Failed to create product.');
          return;
        }

        this.alertService.success('Product created successfully.');
        this.router.navigate(['/products']);
      },
      error: (error: ApiError) => {
        applyApiFieldErrors(this.productForm, error);
        this.alertService.error(error.message || 'Failed to create product.');
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

  get name() {
    return this.productForm.controls.name;
  }

  get description() {
    return this.productForm.controls.description;
  }

  get price() {
    return this.productForm.controls.price;
  }

  get quantity() {
    return this.productForm.controls.quantity;
  }

  backendErrors(controlName: string): string[] {
    return getBackendErrors(this.productForm, controlName);
  }
}
