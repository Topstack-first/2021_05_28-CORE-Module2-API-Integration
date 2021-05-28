//Libraries goes down here
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppConfigService } from './config/app-config-service';
import { HttpConfigInterceptor } from './interceptor/httpconfig.interceptor';
import { ToastrModule } from 'ngx-toastr';

//Components goes down here
import { AppComponent } from './app.component';
import { LoginComponent } from "./views/login/login.component";
import { CreateAccountComponent } from './views/create-account/create-account.component';
import { ForgotPasswordComponent } from "./views/forgot-password/forgot-password.component";
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { SidenavLayoutComponent } from './layouts/sidenav-layout/sidenav-layout.component';
import { ToastrComponent } from './shared/toastr/toastr.component';

//Module goes down here
import { AuthLogoModule } from './shared/auth-logo/auth-logo.module';
import { NavUserInfoModule } from './shared/nav-user-info/nav-user-info.module';
import { AppRoutingModule } from './app-routing.module';

//Helper goes down here
import { authInterceptorProviders } from './_helpers/auth.interceptor';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule, 
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      toastComponent: ToastrComponent,
      enableHtml: true,
      timeOut: 10000
    }),
    AuthLogoModule,
    NavUserInfoModule,
    RouterModule
  ],
  declarations: [
    AppComponent,
		LoginComponent,
		ForgotPasswordComponent,
		CreateAccountComponent,
		HomeLayoutComponent,
		SidenavLayoutComponent,
    ToastrComponent
  ],
  providers: [
    authInterceptorProviders,
    AppConfigService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class AppModule { }
