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
import { ProductStatus } from '../models/product-status.enum';
import { ProductStatusHistory } from '../models/product-status-history.model';

export interface ProductFilters {
  name?: string;
  description?: string;
  price?: number | null;
  quantity?: number | null;
}

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

    return this.http.get<ApiResponse<PaginatedResult<ProductListItem>>>(this.apiUrl, { params });
  }

  getProductById(id: number): Observable<ApiResponse<ProductDetails>> {
    return this.http.get<ApiResponse<ProductDetails>>(`${this.apiUrl}/${id}`);
  }

  createProduct(request: CreateProductRequest): Observable<ApiResponse<unknown>> {
    return this.http.post<ApiResponse<unknown>>(this.apiUrl, request);
  }

  deleteProduct(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  updateProductStatus(id: number, status: ProductStatus): Observable<ApiResponse<unknown>> {
    return this.http.patch<ApiResponse<unknown>>(`${this.apiUrl}/${id}/status`, { status });
  }

  getProductStatusHistories(productId: number): Observable<ApiResponse<ProductStatusHistory[]>> {
    return this.http.get<ApiResponse<PaginatedResult<ProductStatusHistory>>>(`${environment.apiBaseUrl}/product-status-histories`, {
      params: new HttpParams().set('productId', productId)
    }).pipe(
      map(response => ({
        ...response,
        data: response.data?.data
          ? [...response.data.data].sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
          : []
      }))
    );
  }
}
