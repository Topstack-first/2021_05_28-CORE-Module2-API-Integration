import { SelectionModel } from '@angular/cdk/collections';
import { Component, Input, Output, EventEmitter, OnInit, Inject, ViewChild, OnDestroy } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, Observable, of, Subscription, throwError } from 'rxjs';
import { catchError, debounceTime, filter, switchMap, tap } from 'rxjs/operators';
import { UserService, UserDto } from '../../../_services/user-service.service';
import { UserDeleteComponent } from '../user-delete/user-delete.component';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.css']
})
export class AllUsersComponent implements OnInit, OnDestroy {
  @Input() formFilter: FormGroup;

  @Input() filters: any;

  @Input() departments: Array<any>;

  @Input() roles: Array<any>;

  @Input() permissions: Array<any>;

  private _subscriptions$: Array<Subscription> = [];

  users: Array<any>;

  bulkActions: Array<any> = [
    {
      Api: 1,
      viewValue: 'Change Approval Status',
      ddownKey: 'Approval Status',
      ddownValue: ['Approved', 'Denied']
    },
    {
      Api: 2,
      viewValue: 'Change Account Status',
      ddownKey: 'Account Status',
      ddownValue: ['Active', 'Inactive']
    },
    {
      Api: 3,
      viewValue: 'Grant Role',
      ddownKey: 'Role',
      ddownValue: []
    },
    {
      Api: 4,
      viewValue: 'Delete User',
    }
  ];
  bulkAction = this.bulkActions[0];

  public selection = new SelectionModel<UserDto>(true, []);

  @ViewChild(MatPaginator) paginator: MatPaginator;

  public displayedColumns = ['select', 'username', 'name', 'email', 'roles', 'status', 'department', 'document attached', 'post', 'last login', 'login times', 'action'];

  public dataSource: any;

  public restApiDataSource$: Observable<UserDto[]>;

  public pageSize: number = 5;

  public pageSizeOptions = [5, 10, 20];

  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private toastrService: ToastrService
  ) { }

  ngOnDestroy() {
    this._subscriptions$.forEach(subscription => {
      subscription.unsubscribe();
    });
  }

  ngOnInit(): void {
    this.initialiseUserData();
    this._onFormFilterValueChange();
  }

  filtersToggle(): void {
    this.filters.toggle();
  }

  initialiseUserData() {
    var promises = [];

    promises.push(this.getUserData());

    Promise.all(promises)
      .then(responses => {
        // all responses are resolved successfully
        return responses;
      })
      // set the values of variables
      .then(responses => {
        this.users = responses[0];
        for (let user of this.users) {
          for (let department of this.departments) {
            if (user.departmentId == department.departmentId) {
              user.department = department.departmentName;
              break;
            }
          }
        }
        for (let user of this.users) {
          user.isSelected = false;
          for (let role of this.roles) {
            if (user.roleId == role.roleId) {
              user.role = role.roleName;
              break;
            }
          }
        }
        console.log('Users: ', this.users);
        this.dataSource = new MatTableDataSource<UserDto>(this.users);
        this.dataSource.paginator = this.paginator;
        this.paginator.showFirstLastButtons = true;
      })
  }

  getUserData() {
    return new Promise((myResolve, myError) => {
      this.userService.GetUsers().subscribe(
        data => {
          myResolve(JSON.parse(data));
        },
        err => {
          myError(JSON.parse(err.error).message);
        },
      );
    });
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

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  pageSizeOptionChange(): void {
    this.paginator._changePageSize(this.pageSize);

  }

  deleteUser(user): void {
    var list: number[] = [];
    list.push(user.userId);
    this.userService.deleteUserbyUserID(list).subscribe(
      data => {
        console.log(data)
      },
      err => {
        console.log(err);;
      },
    );
    console.log('Delete button clicked');
  }

  statusColour(status) {
    if (status) {
      return '#07c420'
    } else {
      return '#c40707'
    }
  }

  deleteDocument(element: any): void {
    const dialogRef = this.dialog.open(UserDeleteComponent, {
      width: '250px',
      data: element
    });

    this._subscriptions$.push(
      dialogRef.afterClosed()
        .pipe(
          filter(user => user),
          switchMap(user => forkJoin([
            of(user),
            this.userService.DeleteUserbyUserID({
              userId: [user.userId],
              deleteStatus: true
            })
          ])),
          tap(([user, response]) => {
            let deletedUser = this.users;

            deletedUser.forEach((u, i) => {
              if (u.userId === user.userId) {
                deletedUser.splice(i, 1);
              }
            });

            this.dataSource = new MatTableDataSource<UserDto>(deletedUser);
            this.dataSource.paginator = this.paginator;
            this.toastrService.success(response[0], 'User Delete');
          }),
          catchError(error => {
            this.toastrService.error(error.error.title, 'User Delete');
            return throwError(error);
          })
        )
        .subscribe()
    )
  }

  apply(): void {
    var tempList: Array<Object> = [];
    for (let user of this.selection.selected) {
      tempList.push(JSON.parse(JSON.stringify(user)));
    };
    if (this.bulkAction.Api === 3) {
      this.bulkAction.ddownValue = this.roles;
    }
    var tempData = {
      userList: tempList,
      dialogInfo: this.bulkAction
    };
    let dialogRef = this.dialog.open(BulkActionDialog, {
      data: tempData
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('results after close dialog:', result);
      if (result.flag) {
        this.initialiseUserData();
        this.selection = new SelectionModel<UserDto>(true, []);
      }
    });
    // dialogRef.afterClosed().subscribe(result => {
    // });
    console.log('Temp Data: ', tempData);
  }

  private _onFormFilterValueChange = () => {
    this._subscriptions$.push(
      this.formFilter.valueChanges
        .pipe(
          debounceTime(300),
          tap(formValue => {
            let filteredUser = this.users;

            if (formValue && formValue.search) {
              filteredUser = filteredUser.filter(user => user.userEmail.includes(formValue.search));
            }

            if (formValue && formValue.selectedRole) {
              filteredUser = filteredUser.filter(user => user.roleId === formValue.selectedRole);
            }

            if (formValue && formValue.selectedDepartment) {
              filteredUser = filteredUser.filter(user => user.departmentId === formValue.selectedDepartment);
            }

            if (formValue && formValue.selectedApproval) {
              filteredUser = filteredUser.filter(user => user.userApprovalStatus === formValue.selectedApproval);
            }

            if (formValue && formValue.selectedStatus && formValue.selectedStatus !== 'All') {
              const active = formValue.selectedStatus === 'Active' ? true : false;

              filteredUser = filteredUser.filter(user => user.userAccountStatus === active);
            }

            this.dataSource = new MatTableDataSource<UserDto>(filteredUser);
            this.dataSource.paginator = this.paginator;
          })
        )
        .subscribe()
    );
  };
}

@Component({
  selector: 'bulk-action-dialog',
  templateUrl: 'bulk-action-dialog.html',
  styleUrls: ['./all-users.component.css']
})
export class BulkActionDialog {
  title: string;
  ddownKey: string;
  ddownValue: Array<string>;
  api: number;
  users: Array<any>;
  selectedValue: any;


  constructor(
    private userService: UserService,
    public dialogRef: MatDialogRef<BulkActionDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.title = data.dialogInfo.viewValue;
    this.ddownKey = data.dialogInfo.ddownKey;
    this.ddownValue = data.dialogInfo.ddownValue;
    this.api = data.dialogInfo.Api;
    this.users = data.userList
  }

  apply(): void {
    var tempIdList: Array<number> = [];
    for (let user of this.users) {
      tempIdList.push(user.userId);
    }
    switch (this.api) {
      case 1:
        this.userService.updateApprovalStatusbyUserID(tempIdList, this.selectedValue).subscribe(
          data => {
            this.dialogRef.close({ flag: true });
          },
          err => {
            console.log(err);
            alert("update approval: " + err.error);
          },
        );
        break;
      case 2:
        var temp: Boolean;
        if (this.selectedValue == 'Active') {
          temp = true;
        } else if (this.selectedValue == 'Inactive') {
          temp = false;
        }
        this.userService.updateAccountStatusbyUserId(tempIdList, temp).subscribe(
          data => {
            this.dialogRef.close({ flag: true });
          },
          err => {
            console.log(err);
            alert("update account status: " + err.error);
          },
        );
        break;
      case 3:
        this.userService.changeRolebyUserID(tempIdList, this.selectedValue).subscribe(
          data => {
            this.dialogRef.close({ flag: true });
          },
          err => {
            console.log(err);
            alert("update account status: " + err.error);
          },
        );
        break;
      default:
        alert("Something wrong with bulk action function");
    }
  }

  deleteUsers(): void {
    var confirmation = prompt("Please type 'DELETE' to confirm:");
    if (confirmation !== null && confirmation === 'DELETE') {
      var tempIdList: Array<number> = [];
      for (let user of this.users) {
        tempIdList.push(user.userId);
      };
      this.userService.deleteUserbyUserID(tempIdList).subscribe(
        data => {
          this.dialogRef.close({ flag: true });
        },
        err => {
          console.log(err);
          alert("update approval: " + err.error);
        },
      );
    }
  }

  removeUser(index: number) {
    this.users.splice(index, 1);
  }

  close(): void {
    this.dialogRef.close({ flag: false });
  }
}
