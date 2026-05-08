import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApiResponse } from '../../../../core/models/api-response.model';
import { UserResponse } from '../models/user-response.model';



@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  getUserById(id: string): Observable<ApiResponse<UserResponse>> {
    return this.http.get<ApiResponse<UserResponse>>(
      `${this.apiUrl}/${id}`
    );
  }
}