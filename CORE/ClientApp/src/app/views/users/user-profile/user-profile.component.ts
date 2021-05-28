import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { TokenStorageService, UserService } from '@Core/services';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { __values } from 'tslib';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  public loggedUser$ = {
    userId: 0,
    userName: "",
    firstName: "",
    lastName: "",
    roleId: 0,
    roleName: "",
    userApprovalStatus: null,
    userAccountStatus: null,
    userEmail: "",
    userBiography: "",
    jobTitle: "",
    departmentId: 0,
    department: "",
    userProfilePic: null,
  };
  public userRole = {
    roleId: 0,
    roleName: "",
    rolePermission: 0
  };
  private permissions: Array<any>;
  private departments: Array<any>;
  public imageUrl : SafeResourceUrl;

  constructor(
    private tokenStorageService: TokenStorageService,
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.buildUserDetail();
  }

  /**
   * Build User Detail
   *
   * @description structure data of user after all APIs are called
   * @private
   */
  private buildUserDetail() {
    var promises = [];

    promises.push(this._getUserDetail());
    promises.push(this.getRoleData());
    promises.push(this.getDepartmentData());
    // promises.push(this.getProfilePicture());

    Promise.all(promises)
      .then(responses => {
        // all responses are resolved successfully
        return responses;
      })
      // set the values of variables
      .then(responses => {
        this.loggedUser$ = responses[0];
        this.permissions = responses[1].permission;
        for (let role of responses[1].role) {
          if (role.roleId === this.loggedUser$.roleId) {
            this.userRole = role;
          }
        }
        this.checkPermission();
        this.departments = responses[2];
        for (let department of this.departments) {
          if (department.departmentId == this.loggedUser$.departmentId) {
            this.loggedUser$.department = department.departmentName;
          }
        }
        this.imageUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.loggedUser$.userProfilePic);
        console.log('ProfilePic:', this.imageUrl);
      })
  };

  /**
   * Get User Detail
   *
   * @description get all user detail and filter it base on `params.userId`
   * @todo: need refactoring - to get specific user by `params.userId`
   * @param params
   * @private
  **/
  private _getUserDetail() {

    const localUser = this.tokenStorageService.getUser();
    var routeUserId = parseInt(this.activatedRoute.snapshot.paramMap.get('userId'), 10);
    const userId = routeUserId ? +routeUserId : localUser.userId;

    return new Promise((myResolve, myError) => {
      this.userService.GetUserbyId(userId).subscribe(
        data => {
          myResolve(data);
        },
        err => {
          myError(err.error.message);
        },
      );
    });
  };

  /**
   * Get User Detail
   *
   * @description get all user detail and filter it base on `params.userId`
   * @todo: need refactoring - to get specific user by `params.userId`
   * @param params
   * @private
  **/
   private getProfilePicture() {

    const localUser = this.tokenStorageService.getUser();
    var routeUserId = parseInt(this.activatedRoute.snapshot.paramMap.get('userId'), 10);
    const userId = routeUserId ? +routeUserId : localUser.userId;

    return new Promise((myResolve, myError) => {
      this.userService.GetUserProfilePicbyId(userId).subscribe(
        data => {
          myResolve(data);
        },
        err => {
          myError(err.error.message);
        },
      );
    });
  };

  /**
   * Get Role Detail
   *
   * @description get all role and role capabilities
   * @param params
   * @private
   */
  getRoleData() {
    return new Promise((myResolve, myError) => {
      this.userService.GetRolesAndPermission().subscribe(
        data => {
          myResolve(JSON.parse(data));
        },
        err => {
          myError(JSON.parse(err.error).message);
        }
      );
    });
  }

  /**
   * Get Department Detail
   *
   * @description get all department data
   * @param params
   * @private
   */
  getDepartmentData() {
    return new Promise((myResolve, myError) => {
      this.userService.GetDepartments().subscribe(
        data => {
          myResolve(JSON.parse(data));
        },
        err => {
          myError(JSON.parse(err.error).message);
        }
      );
    });
  }

  /**
   * Check User Permission
   *
   * @description get all role and role capabilities
   * @todo: fix logic for checking if user has permission
   * @param params
   * @private
   */
  checkPermission() {
    var flag = (this.userRole.rolePermission >>> 0).toString(2).split("").reverse().join("");
    var arr = [];
    var tempPermission = JSON.parse(JSON.stringify(this.permissions))
    for (var i = 0; i < tempPermission.length; i++) {
      var bitLength = (tempPermission[i].bitWiseValue >>> 0).toString(2).length;
      if (flag.charAt(bitLength-1) == '1') {
        arr.push(tempPermission[i]);
      }
    }
    this.permissions = arr;
  }

  showCapabilities(): void {
    var tempData = {
      user: "",
      permissions: this.permissions,
      role: this.userRole
    };
    tempData.user = this.loggedUser$.userName;
    let dialogRef = this.dialog.open(UserCapabilitiesDialog, {
      data: tempData
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }
}

@Component({
  selector: 'user-capabilities-dialog',
  templateUrl: 'user-capabilities.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserCapabilitiesDialog {
  user: String;
  permissions: Array<any>;
  role: any;


  constructor(
    public dialogRef: MatDialogRef<UserCapabilitiesDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.permissions = data.permissions;
    this.user = data.user;
    this.role = data.role;
  }

  close(): void {
    this.dialogRef.close({ flag: false });
  }
}

