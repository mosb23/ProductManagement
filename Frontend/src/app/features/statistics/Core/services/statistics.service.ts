import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiResponse } from '../../../../core/models/api-response.model';
import { StatisticsResponse } from '../models/statistics-response.model';
import { environment } from '../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/statistics`;

  getStatistics(): Observable<ApiResponse<StatisticsResponse>> {
    return this.http.get<ApiResponse<StatisticsResponse>>(this.apiUrl);
  }
}