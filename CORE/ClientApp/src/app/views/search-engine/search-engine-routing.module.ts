import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchEngineComponent } from './search-engine/search-engine.component';

const routes: Routes = [
  {
    path: '',
    component: SearchEngineComponent,
    children: [
     //{path: 'categories', component: CategoriesComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SearchEngineRoutingModule { }
