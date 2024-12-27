import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdatePantientDto, PantientDto } from '../pantient-dtos/models';

@Injectable({
  providedIn: 'root',
})
export class PantientService {
  apiName = 'Default';
  

  create = (input: CreateUpdatePantientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PantientDto>({
      method: 'POST',
      url: '/api/app/pantient',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/pantient/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PantientDto>({
      method: 'GET',
      url: `/api/app/pantient/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PantientDto>>({
      method: 'GET',
      url: '/api/app/pantient',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdatePantientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PantientDto>({
      method: 'PUT',
      url: `/api/app/pantient/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
