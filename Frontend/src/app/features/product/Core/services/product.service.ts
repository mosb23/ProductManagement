import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { CreateProductRequest } from '../models/create-product-request.model';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../core/models/api-response.model';
import { PaginatedResult } from '../models/paginated-result.model';
import { ProductDetails } from '../models/product-details.model';
import { ProductListItem } from '../models/product-list-item.model';

export interface ProductFilters {
  name?: string;
  description?: string;
  price?: number | null;
  quantity?: number | null;
}

const PRODUCT_STATUS_LABELS: Record<number, string> = {
  0: 'Available',
  1: 'Out of Stock',
  2: 'Discontinued',
  3: 'Pre Order',
  4: 'Back Order',
  5: 'Draft'
};

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/products`;

  getProducts(
    pageNumber: number,
    pageSize: number,
    filters: ProductFilters = {}
  ): Observable<ApiResponse<PaginatedResult<ProductListItem>>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (filters.name?.trim()) {
      params = params.set('name', filters.name.trim());
    }

    if (filters.description?.trim()) {
      params = params.set('description', filters.description.trim());
    }

    if (filters.price !== null && filters.price !== undefined && !Number.isNaN(filters.price)) {
      params = params.set('price', filters.price);
    }

    if (filters.quantity !== null && filters.quantity !== undefined && !Number.isNaN(filters.quantity)) {
      params = params.set('quantity', filters.quantity);
    }

    return this.http.get<ApiResponse<PaginatedResult<ProductListItem>>>(this.apiUrl, {
      params
    }).pipe(
      map(response => ({
        ...response,
        data: response.data
          ? {
              ...response.data,
              data: response.data.data.map(product => this.normalizeProductStatus(product))
            }
          : response.data
      }))
    );
  }

  getProductById(id: number): Observable<ApiResponse<ProductDetails>> {
    return this.http.get<ApiResponse<ProductDetails>>(`${this.apiUrl}/${id}`)
      .pipe(
        map(response => ({
          ...response,
          data: response.data ? this.normalizeProductStatus(response.data) : response.data
        }))
      );
  }

  createProduct(request: CreateProductRequest): Observable<ApiResponse<unknown>> {
    return this.http.post<ApiResponse<unknown>>(this.apiUrl, request);
  }

  deleteProduct(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  private normalizeProductStatus<T extends ProductListItem | ProductDetails>(product: T): T {
    const status = product.status;

    if (typeof status !== 'number') {
      return product;
    }

    return {
      ...product,
      status: PRODUCT_STATUS_LABELS[status] || 'N/A'
    };
  }
}
