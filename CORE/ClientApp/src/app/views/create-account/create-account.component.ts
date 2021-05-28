import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user-service.service';
import { UtilityService } from '../../_services/utility-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'view-auth',
  templateUrl: './create-account.component.html',
})
export class CreateAccountComponent implements OnInit {
  departments: Array<any>;
  password: string = "";
  confirmPassword: string = "";
  userObject = {
    "userName": "",
    "userEmail": "",
    "firstName": "",
    "lastName": "",
    "userPassword": "",
    "DepartmentId": 0
  }

  constructor(
    private userService: UserService,
    private utilityService: UtilityService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.getDepartmentData();
  }

  getDepartmentData() {
    this.userService.GetDepartments().subscribe(
      data => {
        this.departments = JSON.parse(data);
      },
      err => {
        console.log(JSON.parse(err.error).message);
      }
    );
  }

  signUp(): void {
    if (this.password !== this.confirmPassword) {
      this.toastrService.error('Passwords do not match', 'Sign up error!');
    } else if (!this.utilityService.validateEmail(this.userObject.userEmail)) {
      this.toastrService.error('The email provided is invalid', 'Sign up error!');
    } else if (this.utilityService.validatePassword(this.password).length > 0) {
      var tempArr = this.utilityService.validatePassword(this.password);
      var br = '<br>';
      var errorStr = '';
      while (tempArr.length > 0) {
        errorStr = errorStr.concat(tempArr.pop(), br)
      }
      this.toastrService.error(errorStr, 'Sign up error!');
    }
    else {
      this.userObject.userPassword = this.password;
      this.userService.newUserbyUser(this.userObject).subscribe(
        data => {
          if (data.error != null) {
            this.toastrService.error(data.error, 'Sign up error!');
          } else {
            console.log(data)
            this.toastrService.success('You have successfully signed up. Please wait for admin approval.', 'Sign up successful!');
            this.password = "";
            this.confirmPassword = "";
            this.userObject = {
              "userName": "",
              "userEmail": "",
              "firstName": "",
              "lastName": "",
              "userPassword": "",
              "DepartmentId": 0
            }
          }
        },
        err => {
          console.log(err);
          this.toastrService.error(err.error, 'Sign up error!');
        },
      );
    }
  }
}
