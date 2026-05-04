import { Pipe, PipeTransform } from '@angular/core';
import { ProductStatus } from '../models/product-status.enum';
import { getProductStatusOption } from '../utils/product-status.util';

@Pipe({
  name: 'productStatus',
  standalone: true
})
export class ProductStatusPipe implements PipeTransform {
  transform(value: ProductStatus | string | null | undefined, mode: 'label' | 'badgeClass' = 'label'): string {
    const option = getProductStatusOption(value);

    if (!option) {
      return mode === 'badgeClass' ? 'bg-slate-100 text-slate-700' : 'N/A';
    }

    return mode === 'badgeClass' ? option.badgeClass : option.label;
  }
}
