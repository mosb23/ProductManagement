import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../models/api-response.model';
import { LoginRequest } from '../../../features/user/login/Core/models/login-request.model';
import { LoginResponse } from '../../../features/user/login/Core/models/login-response.model';
import { AuthUser } from '../../models/auth-user.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly storageKey = 'product_management_auth_user';

  currentUser = signal<AuthUser | null>(this.loadUserFromStorage());

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  login(request: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http
      .post<ApiResponse<LoginResponse>>(`${environment.apiBaseUrl}/auth/login`, request)
      .pipe(
        tap(response => {
          if (response.success && response.data) {
            const user: AuthUser = {
              id: response.data.id,
              fullName: response.data.fullName,
              email: response.data.email,
              role: response.data.role,
              claims: response.data.claims ?? [],
              token: response.data.token,
              expiresAt: response.data.expiresAt
            };

            localStorage.setItem(this.storageKey, JSON.stringify(user));
            this.currentUser.set(user);
          }
        })
      );
  }

  logout(): void {
    this.clearSession();
    this.router.navigate(['/login']);
  }

  clearSession(): void {
    localStorage.removeItem(this.storageKey);
    this.currentUser.set(null);
  }

  isLoggedIn(): boolean {
    const user = this.currentUser();

    if (!user?.token) {
      return false;
    }

    return new Date(user.expiresAt) > new Date();
  }

  getCurrentUser(): AuthUser | null {
    return this.currentUser();
  }

  getToken(): string | null {
    return this.currentUser()?.token ?? null;
  }

  private loadUserFromStorage(): AuthUser | null {
    const storedUser = localStorage.getItem(this.storageKey);

    if (!storedUser) {
      return null;
    }

    try {
      const user = JSON.parse(storedUser) as AuthUser;

      if (!user.token || new Date(user.expiresAt) <= new Date()) {
        localStorage.removeItem(this.storageKey);
        return null;
      }

      return user;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}