import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../../../environments/environment';
import { ApiResponse } from '../../../../../core/models/api-response.model';
import { PaginatedResult } from '../models/paginated-result.model';
import { ProductListItem } from '../models/product-list-item.model';

@Injectable({
  providedIn: 'root'
})
export class ProductListService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/products`;

  getProducts(
    pageNumber: number,
    pageSize: number,
    name?: string
  ): Observable<ApiResponse<PaginatedResult<ProductListItem>>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (name?.trim()) {
      params = params.set('name', name.trim());
    }

    return this.http.get<ApiResponse<PaginatedResult<ProductListItem>>>(this.apiUrl, {
      params
    });
    
  }

  deleteProduct(id: number) {
  return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }
}