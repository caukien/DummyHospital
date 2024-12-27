import type { DistrictLevel } from '../area/district-level.enum';
import type { FullAuditedEntityDto } from '@abp/ng.core';

export interface CreateUpdateDistrictDto {
  code: number;
  districtName: string;
  districtEnName?: string;
  districtLevel: DistrictLevel;
  provinceCode: number;
}

export interface DistrictDto extends FullAuditedEntityDto<string> {
  code: number;
  districtName?: string;
  districtEnName?: string;
  districtLevel: DistrictLevel;
  provinceCode: number;
}
