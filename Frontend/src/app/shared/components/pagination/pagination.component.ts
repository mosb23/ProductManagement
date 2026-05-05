import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss'
})
export class PaginationComponent {
  @Input() pageNumber = 1;
  @Input() totalPages = 1;

  @Output() pageChange = new EventEmitter<number>();

  get pages(): number[] {
    const visiblePages = 5;
    const halfWindow = Math.floor(visiblePages / 2);
    const start = Math.max(1, Math.min(this.pageNumber - halfWindow, this.totalPages - visiblePages + 1));
    const end = Math.min(this.totalPages, start + visiblePages - 1);

    return Array.from({ length: end - start + 1 }, (_, index) => start + index);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.pageNumber) {
      return;
    }

    this.pageChange.emit(page);
  }
}
