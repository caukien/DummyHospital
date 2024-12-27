import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';

import { ProvinceRoutingModule } from './province-routing.module';
import { ProvinceComponent } from './province.component';

@NgModule({
  declarations: [ProvinceComponent],
  imports: [
    CommonModule,
    // ProvinceRoutingModule
    ProvinceRoutingModule,
    SharedModule,
  ],
})
export class ProvinceModule {}
