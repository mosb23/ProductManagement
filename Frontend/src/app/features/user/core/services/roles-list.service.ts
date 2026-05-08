import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';


import { RoleResponse } from '../models/role-response.model';
import { RoleClaimResponse } from '../models/role-claim-response.model';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../core/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class RolesListService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiBaseUrl}/roles`;

  getRoles(
    pageNumber: number,
    pageSize: number
  ): Observable<ApiResponse<any>> {

    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<ApiResponse<any>>(
      this.apiUrl,
      { params }
    );
  }

  getRoleClaims(
    roleId: string
  ): Observable<ApiResponse<RoleClaimResponse[]>> {
    return this.http.get<ApiResponse<RoleClaimResponse[]>>(
      `${this.apiUrl}/${roleId}/claims`
    );
  }
}