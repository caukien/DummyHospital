import type { FullAuditedAggregateRoot } from '../volo/abp/domain/entities/auditing/models';
import type { CommuneLevel } from '../area/commune-level.enum';

export interface CommuneDto extends FullAuditedAggregateRoot<string> {
  communeCode: number;
  communeName?: string;
  communeEnName?: string;
  communeLevel: CommuneLevel;
  districtCode: number;
}

export interface CreateUpdateCommuneDto {
  communeCode: number;
  communeName?: string;
  communeEnName?: string;
  communeLevel: CommuneLevel;
  districtCode: number;
}
