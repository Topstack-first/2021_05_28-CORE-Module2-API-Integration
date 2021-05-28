import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreHealthRoutingModule } from './core-health-routing.module';
import { CoreHealthCheckupComponent } from './core-health-checkup/core-health-checkup.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    CoreHealthCheckupComponent
  ],
  imports: [
    CommonModule,
    CoreHealthRoutingModule,
    SharedModule,
    FormsModule, 
    ReactiveFormsModule,
    RouterModule
  ]
})
export class CoreHealthModule { }
