import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ClaimLabelPipe } from '../../../../shared/pipes/claim-label-pipe';
import { RoleClaimResponse } from '../../core/models/role-claim-response.model';
import { RoleResponse } from '../../core/models/role-response.model';
import { RolesListService } from '../../core/services/roles-list.service';



@Component({
  selector: 'app-roles-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ClaimLabelPipe
  ],
  templateUrl: './roles-list.component.html',
  styleUrl: './roles-list.component.scss'
})
export class RolesListComponent implements OnInit {

  private readonly rolesService = inject(RolesListService);

  roles: RoleResponse[] = [];

  selectedRole: RoleResponse | null = null;

  claims: RoleClaimResponse[] = [];

  isLoading = false;

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {

    this.isLoading = true;

    this.rolesService
      .getRoles(1, 100)
      .subscribe({
        next: response => {

          this.roles = response.data?.data ?? [];

          this.isLoading = false;
        },
        error: () => {
          this.roles = [];
          this.isLoading = false;
        }
      });
  }

  selectRole(role: RoleResponse): void {

    this.selectedRole = role;

    this.rolesService
      .getRoleClaims(role.id)
      .subscribe({
        next: response => {
          this.claims = response.data ?? [];
        },
        error: () => {
          this.claims = [];
        }
      });
  }
}
