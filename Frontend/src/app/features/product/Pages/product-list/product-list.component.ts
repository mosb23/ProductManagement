import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ProductListItem } from '../../Core/models/product-list-item.model';
import { ProductFilters, ProductService } from '../../Core/services/product.service';
import { ProductStatusPipe } from '../../Core/pipes/product-status.pipe';
import { AuthService } from '../../../../core/services/auth/auth.service';
import { HasPermissionDirective } from '../../../../shared/directives/has-permission.directive';


@Component({
  selector: 'app-product-list',
  imports: [CommonModule, ReactiveFormsModule, RouterLink, PaginationComponent, ProductStatusPipe, HasPermissionDirective],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly authService = inject(AuthService);

  readonly canViewStatusHistory = this.authService.hasClaim('product-status-histories:view');

  filtersForm = new FormGroup({
    name: new FormControl('', { nonNullable: true }),
    description: new FormControl('', { nonNullable: true }),
    price: new FormControl<number | null>(null),
    quantity: new FormControl<number | null>(null)
  });

  products: ProductListItem[] = [];

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
        const filters = this.getFiltersFromQueryParams(params);

        this.pageNumber = Number.isNaN(pageParam) || pageParam < 1 ? 1 : pageParam;

        if (pageParam !== this.pageNumber) {
          this.updateQueryParams(this.pageNumber, filters);
          return;
        }

        this.filtersForm.setValue(filters, { emitEvent: false });
        this.loadProducts();
      });

    this.filtersForm.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged((previous, current) => JSON.stringify(previous) === JSON.stringify(current)),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.updateQueryParams(1, this.getCurrentFilters());
      });
  }

  loadProducts(): void {
    this.isLoading = true;

    this.productService
      .getProducts(this.pageNumber, this.pageSize, this.getCurrentFilters())
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
    this.updateQueryParams(page, this.getCurrentFilters());
  }

  resetFilters(): void {
    this.filtersForm.reset({
      name: '',
      description: '',
      price: null,
      quantity: null
    });
  }

  trackByProductId(index: number, product: ProductListItem): number {
    return product.id;
  }

  private updateQueryParams(pageNumber: number, filters: ProductFilters): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        pageNumber,
        pageSize: this.pageSize,
        name: filters.name?.trim() || null,
        description: filters.description?.trim() || null,
        price: filters.price ?? null,
        quantity: filters.quantity ?? null
      },
      queryParamsHandling: 'merge'
    });
  }

  private getCurrentFilters(): ProductFilters {
    const filters = this.filtersForm.getRawValue();

    return {
      name: filters.name,
      description: filters.description,
      price: this.normalizeNumber(filters.price),
      quantity: this.normalizeNumber(filters.quantity)
    };
  }

  private getFiltersFromQueryParams(params: { get: (name: string) => string | null }): Required<ProductFilters> {
    return {
      name: params.get('name') || '',
      description: params.get('description') || '',
      price: this.parseNumberParam(params.get('price')),
      quantity: this.parseNumberParam(params.get('quantity'))
    };
  }

  private parseNumberParam(value: string | null): number | null {
    if (!value) {
      return null;
    }

    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? null : numberValue;
  }

  private normalizeNumber(value: number | null | undefined): number | null {
    return value === null || value === undefined || Number.isNaN(value) ? null : value;
  }
}
