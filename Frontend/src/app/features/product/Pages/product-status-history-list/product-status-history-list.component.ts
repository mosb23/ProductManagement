import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';

import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ProductStatusHistory } from '../../Core/models/product-status-history.model';
import { ProductStatus } from '../../Core/models/product-status.enum';
import { ProductStatusPipe } from '../../Core/pipes/product-status.pipe';
import { ProductStatusHistoryFilters, ProductService } from '../../Core/services/product.service';
import { productStatusOptions } from '../../Core/utils/product-status.util';

interface ProductStatusHistoryFormFilters {
  productId: number | null;
  oldStatus: ProductStatus | null;
  newStatus: ProductStatus | null;
  fromDate: string;
  toDate: string;
}

@Component({
  selector: 'app-product-status-history-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, PaginationComponent, ProductStatusPipe],
  templateUrl: './product-status-history-list.component.html',
  styleUrl: './product-status-history-list.component.scss'
})
export class ProductStatusHistoryListComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef);

  readonly statusOptions = productStatusOptions;

  filtersForm = new FormGroup({
    productId: new FormControl<number | null>(null),
    oldStatus: new FormControl<ProductStatus | null>(null),
    newStatus: new FormControl<ProductStatus | null>(null),
    fromDate: new FormControl('', { nonNullable: true }),
    toDate: new FormControl('', { nonNullable: true })
  });

  histories: ProductStatusHistory[] = [];
  isLoading = false;
  pageNumber = 1;
  pageSize = 10;
  totalPages = 1;
  totalCount = 0;
  errorMessage = '';

  ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(params => {
        const pageParam = Number(params.get('pageNumber') || 1);
        const pageSizeParam = Number(params.get('pageSize') || this.pageSize);
        const filters = this.getFiltersFromQueryParams(params);

        this.pageNumber = Number.isNaN(pageParam) || pageParam < 1 ? 1 : pageParam;
        this.pageSize = Number.isNaN(pageSizeParam) || pageSizeParam < 1 || pageSizeParam > 100 ? 10 : pageSizeParam;

        if (pageParam !== this.pageNumber || pageSizeParam !== this.pageSize) {
          this.updateQueryParams(this.pageNumber, filters);
          return;
        }

        this.filtersForm.setValue(filters, { emitEvent: false });
        this.loadHistories();
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

  loadHistories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.productService
      .getProductStatusHistoryPage(this.pageNumber, this.pageSize, this.getApiFilters())
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.histories = [];
            this.errorMessage = response.message || 'Failed to load status history.';
            return;
          }

          this.histories = response.data.data;
          this.totalCount = response.data.totalCount;
          this.totalPages = response.data.totalPages;
          this.pageNumber = response.data.pageNumber;
          this.pageSize = response.data.pageSize;
        },
        error: () => {
          this.histories = [];
          this.errorMessage = 'Failed to load status history.';
        }
      });
  }

  onPageChange(page: number): void {
    this.updateQueryParams(page, this.getCurrentFilters());
  }

  resetFilters(): void {
    this.filtersForm.reset({
      productId: null,
      oldStatus: null,
      newStatus: null,
      fromDate: '',
      toDate: ''
    });
  }

  toDisplayDate(value: string | null | undefined): string | null {
    if (!value) {
      return null;
    }

    const hasTimezone = /(?:Z|[+-]\d{2}:\d{2})$/i.test(value);
    return hasTimezone ? value : `${value}Z`;
  }

  private updateQueryParams(pageNumber: number, filters: ProductStatusHistoryFilters): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        pageNumber,
        pageSize: this.pageSize,
        productId: filters.productId ?? null,
        oldStatus: filters.oldStatus ?? null,
        newStatus: filters.newStatus ?? null,
        fromDate: filters.fromDate?.trim() || null,
        toDate: filters.toDate?.trim() || null
      },
      queryParamsHandling: 'merge'
    });
  }

  private getApiFilters(): ProductStatusHistoryFilters {
    const filters = this.getCurrentFilters();

    return {
      ...filters,
      fromDate: filters.fromDate ? `${filters.fromDate}T00:00:00` : null,
      toDate: filters.toDate ? `${filters.toDate}T23:59:59` : null
    };
  }

  private getCurrentFilters(): ProductStatusHistoryFormFilters {
    const filters = this.filtersForm.getRawValue();

    return {
      productId: this.normalizeNumber(filters.productId),
      oldStatus: this.normalizeStatus(filters.oldStatus),
      newStatus: this.normalizeStatus(filters.newStatus),
      fromDate: filters.fromDate,
      toDate: filters.toDate
    };
  }

  private getFiltersFromQueryParams(params: { get: (name: string) => string | null }): ProductStatusHistoryFormFilters {
    return {
      productId: this.parseNumberParam(params.get('productId')),
      oldStatus: this.parseStatusParam(params.get('oldStatus')),
      newStatus: this.parseStatusParam(params.get('newStatus')),
      fromDate: params.get('fromDate') || '',
      toDate: params.get('toDate') || ''
    };
  }

  private parseNumberParam(value: string | null): number | null {
    if (!value) {
      return null;
    }

    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? null : numberValue;
  }

  private parseStatusParam(value: string | null): ProductStatus | null {
    const numberValue = this.parseNumberParam(value);
    return this.normalizeStatus(numberValue);
  }

  private normalizeNumber(value: number | null | undefined): number | null {
    return value === null || value === undefined || Number.isNaN(value) ? null : value;
  }

  private normalizeStatus(value: number | null | undefined): ProductStatus | null {
    if (value === null || value === undefined || Number.isNaN(value)) {
      return null;
    }

    return Object.values(ProductStatus).includes(value) ? value as ProductStatus : null;
  }
}
