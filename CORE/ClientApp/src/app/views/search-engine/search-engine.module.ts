import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchEngineRoutingModule } from './search-engine-routing.module';
import { SearchEngineComponent } from './search-engine/search-engine.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    SearchEngineComponent
  ],
  imports: [
    CommonModule,
    SearchEngineRoutingModule,
    SharedModule,
    FormsModule, 
    ReactiveFormsModule,
    RouterModule
  ]
})
export class SearchEngineModule { }
