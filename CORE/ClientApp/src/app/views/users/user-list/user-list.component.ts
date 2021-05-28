import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { UserService } from '@Core/services';

import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, OnDestroy {
  formFilter: FormGroup;

  viewVariable = '';

  departments: Array<any>;

  selectedDepartment: string;

  roles: Array<any>;

  selectedRole: string;

  permissions: Array<any>;

  selectedUser: Object;

  newUser: Boolean;

  selectedApproval: string = "All";

  approvalStatus = [
    "Approved",
    "Denied",
    "Pending"
  ];

  selectedStatus: string = "All";

  accountStatus = [
    "All",
    "Active",
    "Inactive"
  ];

  @Output() filterChange = new EventEmitter(null);

  _subscriptions$: Array<Subscription> = [];

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder
  ) { }

  ngOnDestroy() {
    this._subscriptions$.forEach(subscription => {
      subscription.unsubscribe();
    });
  }

  ngOnInit(): void {
    var promises = [];

    promises.push(this.getDepartmentData());
    promises.push(this.getRoleData());

    Promise.all(promises)
      .then(responses => {
        // all responses are resolved successfully
        return responses;
      })
      // set the values of variables
      .then(responses => {
        this.departments = responses[0];
        this.roles = responses[1].role;
        this.permissions = responses[1].permission;
        this.viewVariable = 'allUsers'
      })

    this._initFormFilter();
  }

  receiveUser($event) {
    this.selectedUser = $event;
    this.newUser = false;
    this.viewVariable = 'userInfo'
  }

  newUserEvent() {
    this.selectedUser = {
      departmentId: 0,
      firstName: "",
      lastName: "",
      jobTitle: "",
      roleId: 4,
      userAccountStatus: false,
      userApprovalStatus: "Pending",
      userBiography: "",
      userEmail: "",
      userName: ""
    };
    this.newUser = true;
    this.viewVariable = 'userInfo'
  }

  cancelEdit() {
    this.viewVariable = 'allUsers'
  }

  updateUser() {
    this.viewVariable = 'allUsers'
    alert("User info updated successfully.");
  }

  newUserCreation() {
    this.viewVariable = 'allUsers'
    alert("New user created successfully.");
  }

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

  private _initFormFilter = () => {
    this.formFilter = this.formBuilder.group({
      search: null,
      selectedRole: null,
      selectedDepartment: null,
      selectedApproval: null,
      selectedStatus: null
    });
  };
}
