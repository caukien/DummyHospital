import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CommuneDto, CreateUpdateCommuneDto } from '../commune-dtos/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class CommuneService {
  apiName = 'Default';

  create = (input: CreateUpdateCommuneDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>(
      {
        method: 'POST',
        url: '/api/app/commune',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/commune/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>(
      {
        method: 'GET',
        url: `/api/app/commune/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getCommuneByDistrictCodeByDistrictCode = (districtCode: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto[]>(
      {
        method: 'GET',
        url: '/api/app/commune/commune-by-district-code',
        params: { districtCode },
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CommuneDto>>(
      {
        method: 'GET',
        url: '/api/app/commune',
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  importFileByFile = (file: any, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/commune/import-file',
        body: file,
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: string, input: CreateUpdateCommuneDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommuneDto>(
      {
        method: 'PUT',
        url: `/api/app/commune/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
