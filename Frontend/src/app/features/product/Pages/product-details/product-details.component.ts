import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ProductDetails } from '../../Core/models/product-details.model';
import { ProductService } from '../../Core/services/product.service';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { AlertService } from '../../../../core/services/alert.service';
import { ApiError } from '../../../../core/models/api-error.model';




@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterLink, ConfirmDialogComponent],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly alertService = inject(AlertService);

  product: ProductDetails | null = null;
  isDeleteDialogOpen = false;
  isDeleting = false;
  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (Number.isNaN(id) || id < 1) {
      this.errorMessage = 'Invalid product id.';
      return;
    }

    this.loadProduct(id);
  }

  private loadProduct(id: number): void {
    this.isLoading = true;

    this.productService.getProductById(id)
      .pipe(
        finalize(() => this.isLoading = false),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Product not found.';
            return;
          }

          this.product = response.data;
        },
        error: () => {
          this.errorMessage = 'Product not found or could not be loaded.';
        }
      });
  }

  openDeleteDialog(): void {
    this.isDeleteDialogOpen = true;
  }

  closeDeleteDialog(): void {
    if (this.isDeleting) {
      return;
    }

    this.isDeleteDialogOpen = false;
  }

  confirmDelete(): void {
    if (!this.product) {
      return;
    }

    this.isDeleting = true;
    const productId = this.product.id;

    this.productService.deleteProduct(productId)
      .pipe(
        finalize(() => this.isDeleting = false),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success) {
            this.alertService.error(response.message || 'Failed to delete product.');
            return;
          }

          this.alertService.success('Product deleted successfully.');
          this.isDeleteDialogOpen = false;
          this.router.navigate(['/products']);
        },
        error: (error: ApiError) => {
          if (error.status === 404) {
            this.alertService.error('Product was already deleted or no longer exists.');
            this.isDeleteDialogOpen = false;
            this.router.navigate(['/products']);
            return;
          }

          this.alertService.error(error.message || 'Failed to delete product.');
        }
      });
  }
}
