import type { FullAuditedAggregateRoot } from '../volo/abp/domain/entities/auditing/models';

export interface CreateUpdatePantientDto {
  code: string;
  name: string;
  provinceCode: number;
  districtCode: number;
  communeCode: number;
  address: string;
  hospitalId?: string;
}

export interface PantientDto extends FullAuditedAggregateRoot<string> {
  code?: string;
  name?: string;
  provinceCode: number;
  districtCode: number;
  communeCode: number;
  address?: string;
  hospitalId?: string;
}
