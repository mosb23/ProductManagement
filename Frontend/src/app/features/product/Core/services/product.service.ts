import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

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

export interface ProductStatusHistoryFilters {
  productId?: number | null;
  oldStatus?: ProductStatus | null;
  newStatus?: ProductStatus | null;
  fromDate?: string | null;
  toDate?: string | null;
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

  getProductStatusHistoryPage(
    pageNumber: number,
    pageSize: number,
    filters: ProductStatusHistoryFilters = {}
  ): Observable<ApiResponse<PaginatedResult<ProductStatusHistory>>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (filters.productId !== null && filters.productId !== undefined && !Number.isNaN(filters.productId)) {
      params = params.set('productId', filters.productId);
    }

    if (filters.oldStatus !== null && filters.oldStatus !== undefined && !Number.isNaN(filters.oldStatus)) {
      params = params.set('oldStatus', filters.oldStatus);
    }

    if (filters.newStatus !== null && filters.newStatus !== undefined && !Number.isNaN(filters.newStatus)) {
      params = params.set('newStatus', filters.newStatus);
    }

    if (filters.fromDate?.trim()) {
      params = params.set('fromDate', filters.fromDate.trim());
    }

    if (filters.toDate?.trim()) {
      params = params.set('toDate', filters.toDate.trim());
    }

    return this.http.get<ApiResponse<PaginatedResult<ProductStatusHistory>>>(`${environment.apiBaseUrl}/product-status-histories`, { params });
  }

}
