import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientComponent } from './patient.component';
import { SharedModule } from '../shared/shared.module';
import { PatientRoutingModule } from './patient-routing.module';

@NgModule({
  declarations: [PatientComponent],
  imports: [PatientRoutingModule, SharedModule],
})
export class PatientModule {}
