import { mapEnumToOptions } from '@abp/ng.core';

export enum ProductType {
  Single = 1,
  Grouped = 2,
  Configurable = 3,
  Bundel = 4,
  Virtual = 5,
  Downloadable = 6,
}

export const productTypeOptions = mapEnumToOptions(ProductType);
