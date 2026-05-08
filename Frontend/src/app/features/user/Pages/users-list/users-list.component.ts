import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { UserResponse } from '../../core/models/user-response.model';
import { UsersListService } from '../../core/services/users-list.service';
import { HasPermissionDirective } from '../../../../shared/directives/has-permission.directive';



@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    PaginationComponent,
    HasPermissionDirective
  ],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.scss'
})
export class UsersListComponent implements OnInit {

  private readonly usersService = inject(UsersListService);
  private readonly destroyRef = inject(DestroyRef);

  searchControl = new FormControl('', { nonNullable: true });

  users: UserResponse[] = [];

  isLoading = false;

  pageNumber = 1;
  pageSize = 10;
  totalPages = 1;

  ngOnInit(): void {
    this.loadUsers();

    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(() => {
        this.pageNumber = 1;
        this.loadUsers();
      });
  }

  loadUsers(): void {
    this.isLoading = true;

    this.usersService
      .getUsers(
        this.pageNumber,
        this.pageSize,
        this.searchControl.value
      )
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: response => {
          this.users = response.data?.data ?? [];
          this.totalPages = response.data?.totalPages ?? 1;
        },
        error: () => {
          this.users = [];
        }
      });
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadUsers();
  }

  trackByUser(index: number, user: UserResponse): string {
    return user.id;
  }
}
