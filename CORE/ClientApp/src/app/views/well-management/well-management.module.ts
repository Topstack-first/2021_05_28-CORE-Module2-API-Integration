import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WellManagementRoutingModule } from './well-management-routing.module';
import { WellManagementComponent } from './well-management/well-management.component';
import { AddDetailsComponent } from './add-details/add-details.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    WellManagementComponent,
    AddDetailsComponent
  ],
  imports: [
    CommonModule,
    WellManagementRoutingModule,
    SharedModule,
    FormsModule, 
    ReactiveFormsModule,
    RouterModule
  ]
})
export class WellManagementModule { }
