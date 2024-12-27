import type { ProvinceLevel } from '../area/province-level.enum';
import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateProvinceDto {
  code: number;
  provinceName?: string;
  provinceEnName?: string;
  provinceLevel: ProvinceLevel;
}

export interface ProvinceDto extends FullAuditedEntityDto<string> {
  code: number;
  provinceName?: string;
  provinceEnName?: string;
  provinceLevel: ProvinceLevel;
}
