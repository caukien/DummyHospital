import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateDistrictDto, DistrictDto } from '../district-dtos/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class DistrictService {
  apiName = 'Default';

  create = (input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'POST',
        url: '/api/app/district',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/district/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'GET',
        url: `/api/app/district/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getDistrictByProvinceCodeByProvinceCode = (provinceCode: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto[]>(
      {
        method: 'GET',
        url: '/api/app/district/district-by-province-code',
        params: { provinceCode },
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DistrictDto>>(
      {
        method: 'GET',
        url: '/api/app/district',
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
        url: '/api/app/district/import-file',
        body: file,
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: string, input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>(
      {
        method: 'PUT',
        url: `/api/app/district/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
