import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertService } from '../../../core/services/alert.service';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent {
  alertService = inject(AlertService);

  close(): void {
    this.alertService.clear();
  }
}