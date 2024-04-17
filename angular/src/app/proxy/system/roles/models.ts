import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateRoleDto {
  name?: string;
  desciption?: string;
}

export interface RoleDto extends EntityDto<string> {
  name?: string;
  desciption?: string;
}

export interface RoleInListDto extends EntityDto<string> {
  name?: string;
  desciption?: string;
}
