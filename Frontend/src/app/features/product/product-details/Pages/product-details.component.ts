import { CommonModule } from '@angular/common';
import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ProductDetails } from '../Core/models/product-details.model';
import { ProductDetailsService } from '../Core/services/product-details.service';



@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly productDetailsService = inject(ProductDetailsService);
  private readonly destroyRef = inject(DestroyRef);

  product: ProductDetails | null = null;
  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (Number.isNaN(id) || id < 1) {
      this.errorMessage = 'Invalid product id.';
      return;
    }

    this.loadProduct(id);
  }

  private loadProduct(id: number): void {
    this.isLoading = true;

    this.productDetailsService.getProductById(id)
      .pipe(
        finalize(() => this.isLoading = false),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: response => {
          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Product not found.';
            return;
          }

          this.product = response.data;
        },
        error: () => {
          this.errorMessage = 'Product not found or could not be loaded.';
        }
      });
  }
}