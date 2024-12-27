import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommuneComponent } from './commune.component';
import { SharedModule } from '../shared/shared.module';
import { CommuneRoutingModule } from './commune-routing.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';

@NgModule({
  declarations: [CommuneComponent],
  imports: [
    CommuneRoutingModule,
    SharedModule,
    NzTableModule,
    NzButtonModule,
    NzPaginationModule,
    NzModalModule,
    NzPopconfirmModule,
  ],
})
export class CommuneModule {}
