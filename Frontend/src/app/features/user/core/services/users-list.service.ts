import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { UserResponse } from '../models/user-response.model';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../core/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class UsersListService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  getUsers(
    pageNumber: number,
    pageSize: number,
    fullName?: string,
    email?: string,
    role?: string,
    isActive?: boolean
  ): Observable<ApiResponse<any>> {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (fullName?.trim()) {
      params = params.set('fullName', fullName.trim());
    }

    if (email?.trim()) {
      params = params.set('email', email.trim());
    }

    if (role?.trim()) {
      params = params.set('role', role.trim());
    }

    if (isActive !== undefined && isActive !== null) {
      params = params.set('isActive', isActive);
    }

    return this.http.get<ApiResponse<any>>(this.apiUrl, { params });
  }
}