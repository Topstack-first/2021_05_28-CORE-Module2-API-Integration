import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeepSearchingRoutingModule } from './deep-searching-routing.module';
import { DeepsearchingLayoutComponent } from './deepsearching-layout/deepsearching-layout.component';
import { CategoriesComponent } from './categories/categories.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StakeholderComponent } from './stakeholder/stakeholder.component';
import { DepartmentComponent } from './department/department.component';
import { LocationComponent } from './location/location.component';
import { EventComponent } from './event/event.component';
import { RouterModule } from '@angular/router';
import { SubcategoriesComponent } from './subcategories/subcategories.component';
import { EditDataComponent } from './edit-data/edit-data.component';
import { EditSubcategoryComponent } from './edit-subcategory/edit-subcategory.component';
import { EditStakeholderComponent } from './stakeholder/edit-stakeholder/edit-stakeholder.component';
import { EditdepartmentComponent } from './department/edit-department/edit-department.component';
import { EditLocationComponent } from './location/edit-location/edit-location.component';
import { EditEventComponent } from './event/edit-event/edit-event.component';

@NgModule({
  declarations: [
    DeepsearchingLayoutComponent,
    CategoriesComponent,
    StakeholderComponent,
    DepartmentComponent,
    LocationComponent,
    EventComponent,
    SubcategoriesComponent,
    EditDataComponent,
    EditSubcategoryComponent,
    EditStakeholderComponent,
    EditdepartmentComponent,
    EditLocationComponent,
    EditEventComponent
  ],
  imports: [
    CommonModule,
    DeepSearchingRoutingModule,
    SharedModule,
    FormsModule, 
    ReactiveFormsModule,
    RouterModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DeepSearchingModule { }
