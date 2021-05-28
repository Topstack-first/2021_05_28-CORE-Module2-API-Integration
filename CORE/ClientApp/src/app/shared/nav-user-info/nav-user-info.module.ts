import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavUserInfoComponent } from './nav-user-info.component';
import { SharedModule } from '../shared.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [NavUserInfoComponent],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule
  ],
  exports: [NavUserInfoComponent]
})
export class NavUserInfoModule { }
