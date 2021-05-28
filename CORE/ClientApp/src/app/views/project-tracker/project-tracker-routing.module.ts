import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjectTrackerComponent } from './project-tracker/project-tracker.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectTrackerComponent,
    children: [
      //{ path: 'categories', component: CategoriesComponent},
     
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectTrackerRoutingModule { }
