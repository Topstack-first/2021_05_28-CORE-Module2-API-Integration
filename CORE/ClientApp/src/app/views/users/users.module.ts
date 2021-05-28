import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Modules
import { MaterialModule } from 'src/app/shared/material.module';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Components
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserCapabilitiesDialog } from './user-profile/user-profile.component';
import { UsersComponent } from './users.component';
import { NewUserComponent } from './new-user/new-user.component';
import { UserListComponent } from './user-list/user-list.component';
import { AllUsersComponent } from './all-users/all-users.component';
import { BulkActionDialog } from './all-users/all-users.component';
import { UserUpdateComponent } from './user-update/user-update.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';

// Services
import { UserService } from '../../_services/user-service.service';

export const routes = [
  {
    path: '',
    component: UsersComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
      },
      {
        path: 'list',
        component: UserListComponent,
        data: {
          breadcrumb: ['users', 'list']
        }
      },
      {
        path: 'newuser',
        component: NewUserComponent,
        data: {
          breadcrumb: ['users', 'newuser']
        }
      },
      {
        path: 'profile',
        component: UserProfileComponent,
        data: {
          breadcrumb: ['users', 'profile']
        }
      },
      {
        path: ':userId/profile',
        component: UserProfileComponent,
        data: {
          breadcrumb: ['users', 'profile']
        }
      },
      {
        path: ':userId/update',
        component: UserUpdateComponent,
        data: {
          breadcrumb: ['users', 'update']
        }
      }
    ]
  }
];

@NgModule({
  declarations: [
    UsersComponent,
    UserListComponent,
    UserProfileComponent,
    UserCapabilitiesDialog,
    NewUserComponent,
    AllUsersComponent,
    BulkActionDialog,
    UserUpdateComponent,
    UserDeleteComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
  ],
  providers: [
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'fill' } },
    UserService
  ]
})
export class UsersModule { }
