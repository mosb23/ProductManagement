import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { ProductStatusHistory } from '../../Core/models/product-status-history.model';
import { ProductStatusPipe } from '../../Core/pipes/product-status.pipe';

@Component({
  selector: 'app-product-status-history',
  standalone: true,
  imports: [CommonModule, ProductStatusPipe],
  templateUrl: './product-status-history.component.html',
  styleUrl: './product-status-history.component.scss'
})
export class ProductStatusHistoryComponent {
  @Input({ required: true }) statusHistory: ProductStatusHistory[] = [];

  toDisplayDate(value: string | null | undefined): string | null {
    if (!value) {
      return null;
    }

    // Backend dates can come without timezone info; treat them as UTC.
    const hasTimezone = /(?:Z|[+-]\d{2}:\d{2})$/i.test(value);
    return hasTimezone ? value : `${value}Z`;
  }
}
