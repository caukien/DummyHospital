import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateHospitalDto, HospitalDto } from '../hospital-dtos/models';

@Injectable({
  providedIn: 'root',
})
export class HospitalService {
  apiName = 'Default';
  

  create = (input: CreateUpdateHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'POST',
      url: '/api/app/hospital',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'GET',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HospitalDto>>({
      method: 'GET',
      url: '/api/app/hospital',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'PUT',
      url: `/api/app/hospital/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
