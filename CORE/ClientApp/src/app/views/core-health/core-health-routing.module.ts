import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoreHealthCheckupComponent } from './core-health-checkup/core-health-checkup.component';

const routes: Routes = [
  {
    path: '',
    component: CoreHealthCheckupComponent,
  },
      
  //{ path: 'add-details', component: AddDetailsComponent},
     
    
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreHealthRoutingModule { }
