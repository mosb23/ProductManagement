import { Pipe, PipeTransform } from '@angular/core';

import { getClaimLabel } from '../../core/utils/claim-label.util';

@Pipe({
  name: 'claimLabel',
  standalone: true
})
export class ClaimLabelPipe implements PipeTransform {

  transform(value: string): string {
    return getClaimLabel(value);
  }
}