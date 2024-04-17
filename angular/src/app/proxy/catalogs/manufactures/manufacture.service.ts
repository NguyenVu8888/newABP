import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateManufactureDto, ManufactureDto, ManufactureInListDto } from '../../catalog/manufactures/models';
import type { BaseListFilterDto } from '../../models';

@Injectable({
  providedIn: 'root',
})
export class ManufactureService {
  apiName = 'Default';
  

  create = (input: CreateUpdateManufactureDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufactureDto>({
      method: 'POST',
      url: '/api/app/manufacture',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/manufacture/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/manufacture/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufactureDto>({
      method: 'GET',
      url: `/api/app/manufacture/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ManufactureDto>>({
      method: 'GET',
      url: '/api/app/manufacture',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufactureInListDto[]>({
      method: 'GET',
      url: '/api/app/manufacture/all',
    },
    { apiName: this.apiName,...config });
  

  getListFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ManufactureInListDto>>({
      method: 'GET',
      url: '/api/app/manufacture/filter',
      params: { keyword: input.keyword, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateManufactureDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ManufactureDto>({
      method: 'PUT',
      url: `/api/app/manufacture/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
