import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ProductListItem } from '../Core/models/product-list-item.model';
import { ProductListService } from '../Core/services/product-list.service';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { AlertService } from '../../../../core/services/alert.service';
import { ApiError } from '../../../../core/models/api-error.model';


@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, PaginationComponent , ConfirmDialogComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productListService = inject(ProductListService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly alertService = inject(AlertService);

  searchControl = new FormControl('', { nonNullable: true });
  selectedProductForDelete: ProductListItem | null = null;
  products: ProductListItem[] = [];

  isDeleting = false;
  isLoading = false;
  pageNumber = 1;
  pageSize = 10;
  totalPages = 1;
  totalCount = 0;

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(params => {
        const pageParam = Number(params.get('pageNumber') || 1);
        const searchParam = params.get('name') || '';

        this.pageNumber = Number.isNaN(pageParam) || pageParam < 1 ? 1 : pageParam;

        if (pageParam !== this.pageNumber) {
          this.updateQueryParams(this.pageNumber, searchParam);
          return;
        }

        this.searchControl.setValue(searchParam, { emitEvent: false });
        this.loadProducts();
      });

    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(value => {
        this.updateQueryParams(1, value);
      });
  }

  loadProducts(): void {
    this.isLoading = true;

    this.productListService
      .getProducts(this.pageNumber, this.pageSize, this.searchControl.value)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.products = [];
            return;
          }

          this.products = response.data.data;
          this.totalCount = response.data.totalCount;
          this.totalPages = response.data.totalPages;
          this.pageNumber = response.data.pageNumber;
          this.pageSize = response.data.pageSize;
        },
        error: () => {
          this.products = [];
        }
      });
  }

  onPageChange(page: number): void {
    this.updateQueryParams(page, this.searchControl.value);
  }

  trackByProductId(index: number, product: ProductListItem): number {
    return product.id;
  }

  private updateQueryParams(pageNumber: number, name: string): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        pageNumber,
        pageSize: this.pageSize,
        name: name?.trim() || null
      },
      queryParamsHandling: 'merge'
    });
  }

  openDeleteDialog(product: ProductListItem): void {
  this.selectedProductForDelete = product;
}

closeDeleteDialog(): void {
  if (this.isDeleting) {
    return;
  }

  this.selectedProductForDelete = null;
}

confirmDelete(): void {
  if (!this.selectedProductForDelete) {
    return;
  }

  this.isDeleting = true;
  const productId = this.selectedProductForDelete.id;

  this.productListService.deleteProduct(productId)
    .pipe(finalize(() => this.isDeleting = false))
    .subscribe({
      next: response => {
        if (!response.success) {
          this.alertService.error(response.message || 'Failed to delete product.');
          return;
        }

        this.alertService.success('Product deleted successfully.');
        this.selectedProductForDelete = null;

        this.products = this.products.filter(product => product.id !== productId);

        if (this.products.length === 0 && this.pageNumber > 1) {
          this.onPageChange(this.pageNumber - 1);
          return;
        }

        this.loadProducts();
      },
      error: (error: ApiError) => {
        if (error.status === 404) {
          this.alertService.error('Product was already deleted or no longer exists.');
          this.products = this.products.filter(product => product.id !== productId);
          this.selectedProductForDelete = null;
          return;
        }

        this.alertService.error(error.message || 'Failed to delete product.');
      }
    });
  }
}