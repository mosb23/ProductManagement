import { CommonModule } from '@angular/common';
import { Component, DestroyRef, computed, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ProductDetails } from '../../Core/models/product-details.model';
import { ProductService } from '../../Core/services/product.service';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { AlertService } from '../../../../core/services/alert.service';
import { ApiError } from '../../../../core/models/api-error.model';
import { ProductStatus } from '../../Core/models/product-status.enum';
import { productStatusOptions } from '../../Core/utils/product-status.util';
import { ProductStatusPipe } from '../../Core/pipes/product-status.pipe';
import { ProductStatusHistoryComponent } from '../product-status-history/product-status-history.component';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { HasPermissionDirective } from '../../../../shared/directives/has-permission.directive';




@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterLink, ConfirmDialogComponent, ProductStatusPipe, ProductStatusHistoryComponent, HasPermissionDirective],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly alertService = inject(AlertService);
  private readonly authService = inject(AuthService);

  readonly statusOptions = productStatusOptions;
  readonly productStatus = ProductStatus;
  readonly canViewStatusHistory = computed(() => this.authService.hasClaim('product-status-histories:view'));

  product: ProductDetails | null = null;
  selectedStatus: ProductStatus | null = null;
  isDeleteDialogOpen = false;
  isStatusDialogOpen = false;
  isDeleting = false;
  isUpdatingStatus = false;
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

  private loadProduct(id: number, showLoading = true): void {
    if (showLoading) {
      this.isLoading = true;
    }

    this.productService.getProductById(id)
      .pipe(
        finalize(() => {
          if (showLoading) {
            this.isLoading = false;
          }
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Product not found.';
            return;
          }

          this.product = response.data;
          this.selectedStatus = response.data.status ?? null;
        },
        error: (error: ApiError) => {
          this.errorMessage = error.status === 404
            ? 'Product not found.'
            : 'Product not found or could not be loaded.';
        }
      });
  }

  onSelectedStatusChange(value: string): void {
    const parsedStatus = Number(value);
    this.selectedStatus = Number.isNaN(parsedStatus) ? null : parsedStatus as ProductStatus;
  }

  openStatusDialog(): void {
    if (!this.product || this.selectedStatus === null || this.isUpdatingStatus) {
      return;
    }

    if (this.selectedStatus === this.product.status) {
      this.alertService.info('Status is unchanged. Select a different status to continue.');
      return;
    }

    this.isStatusDialogOpen = true;
  }

  closeStatusDialog(): void {
    if (this.isUpdatingStatus) {
      return;
    }

    this.isStatusDialogOpen = false;
  }

  confirmStatusChange(): void {
    if (!this.product || this.selectedStatus === null || this.selectedStatus === this.product.status) {
      return;
    }

    this.isUpdatingStatus = true;

    this.productService.updateProductStatus(this.product.id, this.selectedStatus)
      .pipe(
        finalize(() => this.isUpdatingStatus = false),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success) {
            this.alertService.error(response.message || 'Failed to update product status.');
            return;
          }

          const productId = this.product!.id;
          this.isStatusDialogOpen = false;
          this.alertService.success('Product status updated successfully.');
          this.loadProduct(productId, false);
        },
        error: (error: ApiError) => {
          if (error.status === 404) {
            this.errorMessage = 'Product not found.';
            this.alertService.error('Product not found.');
            this.router.navigate(['/products']);
            return;
          }

          this.alertService.error(error.message || 'Failed to update product status.');
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

  toDisplayDate(value: string | null | undefined): string | null {
    if (!value) {
      return null;
    }

    // Backend dates can come without timezone info; treat them as UTC.
    const hasTimezone = /(?:Z|[+-]\d{2}:\d{2})$/i.test(value);
    return hasTimezone ? value : `${value}Z`;
  }
}
