import { mapEnumToOptions } from '@abp/ng.core';

export enum CommuneLevel {
  Undifined = 0,
  Phuong = 1,
  Xa = 2,
  Tt = 3,
}

export const communeLevelOptions = mapEnumToOptions(CommuneLevel);
