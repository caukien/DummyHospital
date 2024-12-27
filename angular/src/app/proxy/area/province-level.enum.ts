import { mapEnumToOptions } from '@abp/ng.core';

export enum ProvinceLevel {
  Undefined = 0,
  Tinh = 1,
  TPW = 2,
}

export const provinceLevelOptions = mapEnumToOptions(ProvinceLevel);
