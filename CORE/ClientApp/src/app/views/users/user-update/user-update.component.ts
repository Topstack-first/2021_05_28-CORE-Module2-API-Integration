import { ActivatedRoute } from '@angular/router';
import { TokenStorageService, UserService, UtilityService } from '@Core/services';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.scss']
})
export class UserUpdateComponent implements OnInit {
  departments: Array<any>;
  roles: Array<any>;
  loggedUser: any;
  isLoggedUser: boolean;
  permissions: Array<any>;
  password: string = "";
  confirmPassword: string = "";
  isFormSubmitting: boolean = false;
  url: SafeResourceUrl;
  profilePicChanged: boolean = false;
  msg = "";
  userObject = {
    userId: null,
    userName: "",
    firstName: null,
    lastName: null,
    roleId: null,
    userApprovalStatus: null,
    userAccountStatus: null,
    userEmail: null,
    userBiography: null,
    jobTitle: null,
    departmentId: null,
    userProfilePic: ""
  };
  copyUser = {
    userId: null,
    userName: "",
    firstName: null,
    lastName: null,
    roleId: null,
    userApprovalStatus: null,
    userAccountStatus: null,
    userEmail: null,
    userBiography: null,
    jobTitle: null,
    departmentId: null,
    userProfilePic: ""
  };

  constructor(
    private tokenStorageService: TokenStorageService,
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private utilityService: UtilityService,
    private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getDepartmentData();
    this.getRoleData();
    this.getUserInfo();
    this.loggedUser = this.tokenStorageService.getUser();
  }

  /**
   * Get Department and Role
   *
   * @description get department and role list
   */
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

  /**
   * Get Department and Role
   *
   * @description get department and role list
   */
  getUserInfo() {
    var userId = parseInt(this.activatedRoute.snapshot.paramMap.get('userId'), 10);
    this.userService.GetUserbyId(userId).subscribe(
      data => {
        // console.log(data);
        this.userObject = data;
        if (this.userObject.userProfilePic != null) {
          this.url =  this.sanitizer.bypassSecurityTrustResourceUrl(this.userObject.userProfilePic);
        }
        this.copyUser = JSON.parse(JSON.stringify(data));
      },
      err => {
        this.toastrService.error(JSON.parse(err.error).message, 'Error with getDepartmentData()!');
      }
    );
  }

  /**
   * Update
   *
   * @description update user data
   */
  updateUser(): void {
    var promises = [];
    this.isFormSubmitting = true;
    if (this.password.length > 0 || this.confirmPassword.length > 0) {
      if (this.password !== this.confirmPassword) {
        this.toastrService.error('Passwords do not match', 'Change password error!');
      } else if (this.utilityService.validatePassword(this.password).length > 0) {
        var tempArr = this.utilityService.validatePassword(this.password);
        var br = '<br>';
        var errorStr = '';
        while (tempArr.length > 0) {
          errorStr = errorStr.concat(tempArr.pop(), br)
        }
        this.toastrService.error(errorStr, 'Change password error!');
      } else {
        promises.push(this.updateUserPassword());
      }
    }
    if (!this.utilityService.compareObject(this.userObject, this.copyUser)) {
      if (!this.utilityService.validateEmail(this.userObject.userEmail)) {
        this.toastrService.error('The email provided is invalid', 'Update user error!');
      } else {
        promises.push(this.updateUserInfo());
      }
    }
    if (this.profilePicChanged) {
      promises.push(this.updateUserProfilePic());
    }
    Promise.all(promises)
      // .then(responses => {
      //   // all responses are resolved successfully
      //   return responses;
      // })
      // set the values of variables
      .then(responses => {
        console.log('Response from API: ', responses);
        if (responses.length == 0) {
          this.toastrService.info('No changes were made to the user information.', 'No changes!');
        }
        for (let response of responses) {
          console.log('(typeof response:',typeof response);
          if (typeof response == 'string') {
            this.toastrService.success('Profile picture updated successfully', 'Update picture success!');
            this.profilePicChanged = false;
          }else if (response.length < 5) {
            this.toastrService.success('Password was updated successfully', 'Update password success!');
            this.password = "";
            this.confirmPassword = "";
          } else {
            this.toastrService.success('The user information was updated successfully', 'Update user success!');
            this.getUserInfo();
          }
        }
        this.isFormSubmitting = false;
        console.log(responses);
      })
      .catch(error => {
        this.toastrService.error(error.error, 'Update user error!');
        this.isFormSubmitting = false;
      });
  }
  updateUserInfo() {
    return new Promise((myResolve, myError) => {
      this.userService.updateUser(this.userObject).subscribe(
        data => {
          myResolve(data);
        },
        err => {
          myError(err);
        },
      );
    });
  }

  updateUserPassword() {
    return new Promise((myResolve, myError) => {
      this.userService.updateUserPasswordbyUserID(this.userObject.userId, this.password).subscribe(
        data => {
          myResolve(data);
        },
        err => {
          myError(err);
        },
      );
    });
  }

  updateUserProfilePic() {
    return new Promise((myResolve, myError) => {
      this.userService.updateUserProfilePicbyUserID(this.userObject.userId, this.url).subscribe(
        data => {
          myResolve(data);
        },
        err => {
          myError(err);
        },
      );
    });
  }

  selectFile(event: any) {
    if (!event.target.files[0] || event.target.files[0].length == 0) {
      this.msg = 'You must select an image';
      return;
    }

    var mimeType = event.target.files[0].type;

    if (mimeType.match(/image\/*/) == null) {
      this.msg = "Only images are supported";
      return;
    }

    var reader = new FileReader();
    reader.readAsDataURL(event.target.files[0]);

    reader.onload = (_event) => {
      this.msg = "";
      this.url = reader.result;
      this.profilePicChanged = true;
    }
  }

  goBack() {
    window.history.back();
  }

}
