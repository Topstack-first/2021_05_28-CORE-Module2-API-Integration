import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { UserService } from '../../../_services/user-service.service';
import * as _ from "lodash";
import { UtilityService } from '../../../_services/utility-service.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.css']
})
export class NewUserComponent implements OnInit {

  departments: Array<any>;
  roles: Array<any>;
  permissions: Array<any>;
  password: string = "";
  confirmPassword: string = "";
  isFormSubmitting: boolean = false;
  userObject = {
    "userName": "",
    "userEmail": "",
    "userPost": 0,
    "firstName": "",
    "lastName": "",
    "jobTitle": "",
    "userPassword": "",
    "userBiography": "",
    "departmentId": 0,
    "roleId": 4,
    "DocumentAttached": 0
  }

  constructor(
    private userService: UserService,
    private utilityService: UtilityService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.getDepartmentData();
    this.getRoleData();
  }

  getDepartmentData() {
    this.userService.GetDepartments().subscribe(
      data => {
        this.departments = (JSON.parse(data));
      },
      err => {
        this.toastrService.error(JSON.parse(err.error).message, 'Error with getDepartmentData()!');
      }
    );
  }

  getRoleData() {
    this.userService.GetRolesAndPermission().subscribe(
      data => {
        this.roles = (JSON.parse(data)).role;
      },
      err => {
        this.toastrService.error(JSON.parse(err.error).message, 'Error with getRoleData()!');
      }
    );
  }

  createNewUser(): void {
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
      this.isFormSubmitting = true;
      console.log('userObject from newuser component:', this.userObject);
      this.userService.newUserbyAdmin(this.userObject).subscribe(
        data => {
          if (data.error != null) {
            this.toastrService.error(data.error, 'Sign up error!');
            this.isFormSubmitting = false;
          } else {
            console.log(data)
            this.toastrService.success('You have successfully signed up. Please wait for admin approval.', 'Sign up successful!');
            this.password = "";
            this.confirmPassword = "";
            this.isFormSubmitting = false;
            this.userObject = {
              "userName": "",
              "userEmail": "",
              "userPost": 0,
              "firstName": "",
              "lastName": "",
              "jobTitle": "",
              "userPassword": "",
              "userBiography": "",
              "departmentId": 0,
              "roleId": 4,
              "DocumentAttached": 0
            }
          }
        },
        err => {
          console.log(err);
          this.isFormSubmitting = false;
          this.toastrService.error(err.error, 'Sign up error!');
        },
      );
    }
  }
}
