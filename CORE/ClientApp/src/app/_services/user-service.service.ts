import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from '../config/app-config-service';

const API_URL = AppConfigService.config.API_BASE_URL;
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  GetUsers(): Observable<any> {
    return this.http.get(API_URL + 'GetAllUser', { responseType: 'text' });
  }

  GetUserbyId(userId: number): Observable<any> {
    return this.http.post(API_URL + 'GetUserInfoById',{ "UID": userId }, httpOptions);
  }

  GetUserProfilePicbyId(userId: number): Observable<any> {
    return this.http.post(API_URL + 'GetUserProfilePicbyUserID',{ "UID": userId }, httpOptions);
  }

  GetDepartments(): Observable<any> {
    return this.http.get(API_URL + 'GetDepartments', { responseType: 'text' });
  }

  GetRolesAndPermission(): Observable<any> {
    return this.http.get(API_URL + 'GetRolesAndPermission', { responseType: 'text' });
  }

  updateUser(userObject: any): Observable<any> {
    return this.http.post(API_URL + 'UpdateUserInfobyUserId', userObject, httpOptions);
  }

  //redundant API. Todo: Remove one of the API
  public DeleteUserbyUserID(userObject: any): Observable<any> {
    return this.http.post(`${API_URL}DeleteUserbyUserID`, userObject, httpOptions);
  }

  updateUserPasswordbyUserID(id: number, password: string): Observable<any> {
    return this.http.post(API_URL + 'UpdateUserPasswordbyId', {
      "UserID" : id,
      "NewPassword" : password
    }, httpOptions);
  }

  updateUserProfilePicbyUserID(id: number, base64: any): Observable<any> {
    return this.http.post(API_URL + 'ProfilePicUploadById', {
      "UserId" : id,
      "ImageData" : base64.split(',')[1]
    }, httpOptions);
  }

  newUserbyAdmin(userObject: any): Observable<any> {
    console.log('userObject from service:', userObject);
    return this.http.post(API_URL + 'CreateNewUserbyAdmin', {
      "userName": userObject.userName,
      "userEmail": userObject.userEmail,
      "userPost": 0,
      "firstName": userObject.firstName,
      "lastName": userObject.lastName,
      "jobTitle": userObject.jobTitle,
      "userPassword": userObject.userPassword,
      "userBiography": userObject.userBiography,
      "DepartmentId": userObject.departmentId,
      "roleId": userObject.roleId,
      "DocumentAttached":0
    }, httpOptions);
  }

  newUserbyUser(userObject: any): Observable<any> {
    console.log(userObject);
    return this.http.post(API_URL + 'CreateNewUserbyUser', {
      "userName": userObject.userName,
      "userEmail": userObject.userEmail,
      "firstName": userObject.firstName,
      "lastName": userObject.lastName,
      "userPassword": userObject.userPassword,
      "DepartmentId": userObject.DepartmentId
    }, httpOptions);
  }

  deleteUserbyUserID(idList: Array<number>): Observable<any> {
    return this.http.post(API_URL + 'DeleteUserbyUserID', {
      "UserId" : idList,
      "DeleteStatus" : true
    }, httpOptions);
  }

  updateApprovalStatusbyUserID(idList: Array<number>, status: String): Observable<any> {
    return this.http.post(API_URL + 'UpdateApprovalStatusbyUserID', {
      "UserId" : idList,
      "ApprovalStatus" : status
    }, httpOptions);
  }

  updateAccountStatusbyUserId(idList: Array<number>, status: Boolean): Observable<any> {
    return this.http.post(API_URL + 'UpdateAccountStatusbyUserId', {
      "UserId" : idList,
      "AccountStatus" : status
    }, httpOptions);
  }

  changeRolebyUserID(idList: Array<number>, role: number): Observable<any> {
    return this.http.post(API_URL + 'ChangeRolebyUserID', {
      "UserId" : idList,
      "RoleID" : role
    }, httpOptions);
  }



}

export interface UserDto {
  userId?: number | null;
  userName?: string | null;
  userEmail?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  roleId?: number | null;
  departmentId?: number | null;
  jobTitle?: string | null;
  userBiography?: string | null;
  deleteStatus?: boolean | null;
  documentAttached?: number | null;
  userPost?: number | null;
  userAccountStatus?: boolean | null;
  userApprovalStatus?: boolean | null;
  userProfilePic?: string | null;
  createdBy?: moment.Moment | null;
  createdAt?: moment.Moment | null;
}
