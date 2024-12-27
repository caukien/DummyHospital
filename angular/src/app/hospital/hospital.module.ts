import { NgModule } from '@angular/core';

import { HospitalComponent } from './hospital.component';
import { SharedModule } from '../shared/shared.module';
import { HospitalRoutingModule } from './hospital-routing.module';

@NgModule({
  declarations: [HospitalComponent],
  imports: [HospitalRoutingModule, SharedModule],
})
export class HospitalModule {}
