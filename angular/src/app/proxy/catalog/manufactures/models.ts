import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateManufactureDto {
  name?: string;
  code?: string;
  slug?: string;
  coverPicture?: string;
  visibility: boolean;
  isActive: boolean;
  country?: string;
}

export interface ManufactureDto {
  id?: string;
  name?: string;
  code?: string;
  slug?: string;
  coverPicture?: string;
  visibility: boolean;
  isActive: boolean;
  country?: string;
}

export interface ManufactureInListDto extends EntityDto<string> {
  name?: string;
  code?: string;
  slug?: string;
  coverPicture?: string;
  visibility: boolean;
  isActive: boolean;
  country?: string;
}
