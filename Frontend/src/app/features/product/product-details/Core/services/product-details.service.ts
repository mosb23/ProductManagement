import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../../../environments/environment';
import { ApiResponse } from '../../../../../core/models/api-response.model';
import { ProductDetails } from '../models/product-details.model';

@Injectable({
  providedIn: 'root'
})
export class ProductDetailsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/products`;

  getProductById(id: number): Observable<ApiResponse<ProductDetails>> {
    return this.http.get<ApiResponse<ProductDetails>>(`${this.apiUrl}/${id}`);
  }

  deleteProduct(id: number): Observable<ApiResponse<null>> {
  return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
}
}