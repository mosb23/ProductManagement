import { ProductStatus } from './product-status.enum';

export interface ProductStatusHistory {
  id: number;
  productId: number;
  oldStatus: ProductStatus | null;
  newStatus: ProductStatus;
  createdAt: string;
  createdBy?: string | null;
  productName?: string | null;
}
