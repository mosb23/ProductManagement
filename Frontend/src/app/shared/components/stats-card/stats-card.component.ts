import { Component, Input } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-stats-card',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './stats-card.component.html',
  styleUrl: './stats-card.component.scss'
})
export class StatsCardComponent {
  @Input() title = '';
  @Input() value: number | null = 0;
  @Input() description = '';
  @Input() tone: 'slate' | 'green' | 'yellow' | 'red' | 'indigo' = 'slate';
}