import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { finalize } from 'rxjs';

import { StatsCardComponent } from '../../../shared/components/stats-card/stats-card.component';
import { StatisticsResponse } from '../Core/models/statistics-response.model';
import { StatisticsService } from '../Core/services/statistics.service';


@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule, StatsCardComponent],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.scss'
})
export class StatisticsComponent implements OnInit {
  private readonly statisticsService = inject(StatisticsService);

  statistics: StatisticsResponse | null = null;
  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.statisticsService
      .getStatistics()
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Failed to load statistics.';
            return;
          }

          this.statistics = response.data;
        },
        error: error => {
          this.errorMessage = error.message || 'Failed to load statistics.';
        }
      });
  }
}