import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';


import { CreateUserRequest } from '../models/create-user-request.model';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../core/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AddUserService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  createUser(
    request: CreateUserRequest
  ): Observable<ApiResponse<null>> {
    return this.http.post<ApiResponse<null>>(
      this.apiUrl,
      request
    );
  }
}