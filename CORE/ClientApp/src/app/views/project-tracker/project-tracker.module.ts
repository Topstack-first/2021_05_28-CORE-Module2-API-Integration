import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectTrackerRoutingModule } from './project-tracker-routing.module';
import { ProjectTrackerComponent } from './project-tracker/project-tracker.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    ProjectTrackerComponent
  ],
  imports: [
    CommonModule,
    ProjectTrackerRoutingModule,
    SharedModule,
    FormsModule, 
    ReactiveFormsModule,
    RouterModule
  ]
})
export class ProjectTrackerModule { }
