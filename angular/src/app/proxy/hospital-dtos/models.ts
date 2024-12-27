import type { FullAuditedAggregateRoot } from '../volo/abp/domain/entities/auditing/models';

export interface CreateUpdateHospitalDto {
  hospitalCode: number;
  hospitalName?: string;
  provinceCode: number;
  districtCode: number;
  communeCode: number;
  address?: string;
}

export interface HospitalDto extends FullAuditedAggregateRoot<string> {
  hospitalCode: number;
  hospitalName?: string;
  provinceCode: number;
  districtCode: number;
  communeCode: number;
  address?: string;
}
