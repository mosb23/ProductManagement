import { ProductStatus } from '../models/product-status.enum';

export interface ProductStatusOption {
  value: ProductStatus;
  label: string;
  badgeClass: string;
}

const PRODUCT_STATUS_OPTIONS: ProductStatusOption[] = [
  { value: ProductStatus.Available, label: 'Available', badgeClass: 'bg-emerald-50 text-emerald-700' },
  { value: ProductStatus.OutOfStock, label: 'Out of Stock', badgeClass: 'bg-red-50 text-red-700' },
  { value: ProductStatus.Discontinued, label: 'Discontinued', badgeClass: 'bg-slate-200 text-slate-700' },
  { value: ProductStatus.PreOrder, label: 'Pre Order', badgeClass: 'bg-indigo-50 text-indigo-700' },
  { value: ProductStatus.BackOrder, label: 'Back Order', badgeClass: 'bg-amber-50 text-amber-700' },
  { value: ProductStatus.Draft, label: 'Draft', badgeClass: 'bg-cyan-50 text-cyan-700' }
];

export const productStatusOptions = PRODUCT_STATUS_OPTIONS;

export function getProductStatusOption(status: ProductStatus | string | null | undefined): ProductStatusOption | null {
  if (status === null || status === undefined) {
    return null;
  }

  const statusNumber = typeof status === 'string' ? Number(status) : status;
  if (typeof statusNumber !== 'number' || Number.isNaN(statusNumber)) {
    return null;
  }

  return PRODUCT_STATUS_OPTIONS.find(option => option.value === statusNumber) ?? null;
}
