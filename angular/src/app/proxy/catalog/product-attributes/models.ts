import type { AttributeType } from '../../ecommerce/attributies/attribute-type.enum';
import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateProductAttributeDto {
  code?: string;
  type: AttributeType;
  label?: string;
  sortOrder: number;
  vissibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
  note?: string;
}

export interface ProductAttributeDto {
  code?: string;
  type: AttributeType;
  label?: string;
  sortOrder: number;
  vissibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
  note?: string;
  id?: string;
}

export interface ProductAttributeInListDto extends EntityDto<string> {
  code?: string;
  type: AttributeType;
  label?: string;
  sortOrder: number;
  vissibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
}
