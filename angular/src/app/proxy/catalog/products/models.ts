import type { ProductType } from '../../ecommerce/products/product-type.enum';
import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateProductDto {
  manufictureId?: string;
  code?: string;
  name?: string;
  productType: ProductType;
  sku?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  seoMetaDescription?: string;
  descreption?: string;
  thumbnailPictureName?: string;
  thumbnailPictureContent?: string;
}

export interface ProductDto {
  manufictureId?: string;
  name?: string;
  code?: string;
  productType: ProductType;
  sku?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  seoMetaDescription?: string;
  descreption?: string;
  thumbnailPicture?: string;
  id?: string;
}

export interface ProductInListDto extends EntityDto<string> {
  manufictureId?: string;
  name?: string;
  code?: string;
  productType: ProductType;
  sku?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  thumbnailPicture?: string;
}
