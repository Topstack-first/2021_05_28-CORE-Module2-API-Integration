import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddDetailsComponent } from './add-details/add-details.component';
import { WellManagementComponent } from './well-management/well-management.component';

const routes: Routes = [
  {
    path: '',
    component: WellManagementComponent,
  },
      
  { path: 'add-details', component: AddDetailsComponent},
  { path: 'edit-details', component: AddDetailsComponent},
     
    
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WellManagementRoutingModule { }
