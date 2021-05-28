import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from "./views/login/login.component";
import { CreateAccountComponent } from './views/create-account/create-account.component';
import { ForgotPasswordComponent } from "./views/forgot-password/forgot-password.component";
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { SidenavLayoutComponent } from './layouts/sidenav-layout/sidenav-layout.component';
// import { DeepSearchSettingComponent } from './views/deep-search-setting/deep-search-setting.component';

const appRoutes: Routes = [
	{
		path: '',
		component: HomeLayoutComponent,
	},
	{ 
		path: 'login', 
		component: LoginComponent 
	},
	{ 
		path: 'forgot-password', 
		component: ForgotPasswordComponent 
	},
	{ 
		path: 'create-account', 
		component: CreateAccountComponent 
	},
	{
		path: 'core',
		component: SidenavLayoutComponent,
		children: [
			{ path: 'bulk-uploader', loadChildren: () => import('./views/bulk/bulk.module').then(m => m.BulkModule) },
			{ path: 'document', loadChildren: () => import('./views/document/document.module').then(m => m.DocumentModule) },
			{ path: 'users', loadChildren: () => import('./views/users/users.module').then(m => m.UsersModule) },
			{ path: 'deep-search-setting', loadChildren: () => import('./views/deep-searching/deep-searching.module').then(m => m.DeepSearchingModule) },
			{ path: 'project-tracker-setting', loadChildren: () => import('./views/project-tracker/project-tracker.module').then(m => m.ProjectTrackerModule) },
			{ path: 'well-management', loadChildren: () => import('./views/well-management/well-management.module').then(m => m.WellManagementModule) },
			{ path: 'core-health-checkup', loadChildren: () => import('./views/core-health/core-health.module').then(m => m.CoreHealthModule) },
			{ path: 'admin-settings', loadChildren: () => import('./views/admin-settings/admin-settings.module').then(m => m.AdminSettingsModule) },
			{ path: 'backup-management', loadChildren: () => import('./views/backup-management/backup-management.module').then(m => m.BackupManagementModule)},
			{ path: 'search-engine', loadChildren: () => import('./views/search-engine/search-engine.module').then(m => m.SearchEngineModule)},
		]
	},
	{ path: '**', redirectTo: '' }
];

@NgModule({
	imports: [CommonModule,RouterModule.forRoot(appRoutes)],
	exports: [RouterModule]
})

export class AppRoutingModule { }