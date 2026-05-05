import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnChanges, SimpleChanges, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { ApiError } from '../../../../core/models/api-error.model';
import { ProductStatusHistory } from '../../Core/models/product-status-history.model';
import { ProductStatusPipe } from '../../Core/pipes/product-status.pipe';
import { ProductService } from '../../Core/services/product.service';

@Component({
  selector: 'app-product-status-history',
  standalone: true,
  imports: [CommonModule, ProductStatusPipe],
  templateUrl: './product-status-history.component.html',
  styleUrl: './product-status-history.component.scss'
})
export class ProductStatusHistoryComponent implements OnChanges {
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef);

  @Input({ required: true }) productId!: number;

  statusHistory: ProductStatusHistory[] = [];
  isLoading = false;
  errorMessage = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['productId'] && this.productId > 0) {
      this.loadStatusHistory();
    }
  }

  loadStatusHistory(): void {
    if (!this.productId || this.productId < 1) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.productService.getProductStatusHistories(this.productId)
      .pipe(
        finalize(() => this.isLoading = false),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success) {
            this.statusHistory = [];
            this.errorMessage = response.message || 'Failed to load status history.';
            return;
          }

          this.statusHistory = response.data || [];
        },
        error: (error: ApiError) => {
          this.statusHistory = [];
          this.errorMessage = error.message || 'Failed to load status history.';
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
