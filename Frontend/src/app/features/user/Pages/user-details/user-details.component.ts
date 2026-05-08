import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserDetailsService } from '../../core/services/user-details.service';
import { UserResponse } from '../../core/models/user-response.model';



@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent implements OnInit {

  private readonly route = inject(ActivatedRoute);

  private readonly userService = inject(UserDetailsService);

  user: UserResponse | null = null;

  isLoading = false;

  ngOnInit(): void {

    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      return;
    }

    this.loadUser(id);
  }

  loadUser(id: string): void {

    this.isLoading = true;

    this.userService
      .getUserById(id)
      .subscribe({
        next: response => {

          this.user = response.data ?? null;

          this.isLoading = false;
        },
        error: () => {

          this.user = null;

          this.isLoading = false;
        }
      });
  }
}