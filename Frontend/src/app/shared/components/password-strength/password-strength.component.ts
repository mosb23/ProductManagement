import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { getPasswordStrength } from '../../../core/utils/password-strength.util';

@Component({
  selector: 'app-password-strength',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './password-strength.component.html',
  styleUrl: './password-strength.component.scss'
})
export class PasswordStrengthComponent {

  @Input() password = '';

  get strength() {
    return getPasswordStrength(this.password);
  }
}