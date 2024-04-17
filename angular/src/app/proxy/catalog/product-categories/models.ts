import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateProductCategoryDto {
  name?: string;
  code?: string;
  slug?: string;
  sortOrder: number;
  coverPucture?: string;
  visibility: boolean;
  isActive: boolean;
  parentId?: string;
  seoMetaDescreption?: string;
}

export interface ProductCategoryDto {
  name?: string;
  code?: string;
  slug?: string;
  sortOrder: number;
  coverPucture?: string;
  visibility: boolean;
  isActive: boolean;
  parentId?: string;
  seoMetaDescreption?: string;
  id?: string;
}

export interface ProductCategoryInListDto extends EntityDto<string> {
  name?: string;
  code?: string;
  sortOrder: number;
  coverPucture?: string;
  visibility: boolean;
  isActive: boolean;
}
