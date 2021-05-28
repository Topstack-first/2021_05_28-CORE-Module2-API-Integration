import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import { AuthService } from '../../_services/auth.service';
import { TokenStorageService } from '../../_services/token-storage.service';

@Component({
  selector: 'view-auth',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  form: any = {
    UserNameEmail: null,
    UserPassword: null
  };
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  roles: string[] = [];

  constructor(
    private authService: AuthService, 
    private tokenStorage: TokenStorageService,
    private route: Router
    ) {}

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
      this.roles = this.tokenStorage.getUser().roles;
    }
  }

  login(): void {
    const { UserNameEmail, UserPassword } = this.form;
    
    if (UserNameEmail == null){
      this.errorMessage = "Username is blank.";
      this.isLoginFailed = true;
    }else if (UserPassword == null){
      this.errorMessage = "Password is blank.";
      this.isLoginFailed = true;
    }else{
      this.authService.login(UserNameEmail, UserPassword).subscribe(
      data => {
        this.tokenStorage.saveToken(data.accessToken);
        this.tokenStorage.saveUser(data);
        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.roles = this.tokenStorage.getUser().roles;
        this.route.navigate(['']);
      },
      err => {
        this.errorMessage = err.error.value.error;
        console.log(err);
        this.isLoginFailed = true;
      }
    );
    }
  }

  reloadPage(): void {
    window.location.reload();
  }

}
