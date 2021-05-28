import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesComponent } from './categories/categories.component';
import { DeepsearchingLayoutComponent } from './deepsearching-layout/deepsearching-layout.component';
import { DepartmentComponent } from './department/department.component';
import { EditdepartmentComponent } from './department/edit-department/edit-department.component';
import { EditDataComponent } from './edit-data/edit-data.component';
import { EditSubcategoryComponent } from './edit-subcategory/edit-subcategory.component';
import { EditEventComponent } from './event/edit-event/edit-event.component';
import { EventComponent } from './event/event.component';
import { EditLocationComponent } from './location/edit-location/edit-location.component';
import { LocationComponent } from './location/location.component';
import { EditStakeholderComponent } from './stakeholder/edit-stakeholder/edit-stakeholder.component';
import { StakeholderComponent } from './stakeholder/stakeholder.component';
import { SubcategoriesComponent } from './subcategories/subcategories.component';
const routes: Routes = [
  {
    path: '',
    component: DeepsearchingLayoutComponent,
    children: [
      { path: 'categories', component: CategoriesComponent},
      { path: 'stake', component: StakeholderComponent},
      { path: 'depart', component: DepartmentComponent},
      { path: 'loc', component: LocationComponent},
      { path: 'eve', component: EventComponent},
      { path: 'subcategories', component: SubcategoriesComponent},
      { path: 'editdata', component: EditDataComponent},
      { path: 'createdata', component: EditDataComponent},
      { path: 'editsubcategory', component: EditSubcategoryComponent},
      { path: 'createsubcategory', component: EditSubcategoryComponent},
      { path: 'editstakeholder', component: EditStakeholderComponent},
      { path: 'createstakeholder', component: EditStakeholderComponent},
      { path: 'editdepart', component: EditdepartmentComponent},
      { path: 'createdepart', component: EditdepartmentComponent},
      { path: 'editlocation', component: EditLocationComponent},
      { path: 'createlocation', component: EditLocationComponent},
      { path: 'editevent', component: EditEventComponent},
      { path: 'createevent', component: EditEventComponent},
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeepSearchingRoutingModule { }
