import { mapEnumToOptions } from '@abp/ng.core';

export enum DistrictLevel {
  Undefined = 0,
  Quan = 1,
  Huyen = 2,
  Tx = 3,
  Tp = 4,
}

export const districtLevelOptions = mapEnumToOptions(DistrictLevel);
