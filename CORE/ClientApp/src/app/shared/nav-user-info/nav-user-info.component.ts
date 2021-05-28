import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TokenStorageService, UserService } from '@Core/services';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'nav-user-info',
  templateUrl: './nav-user-info.component.html',
  styleUrls: ['./nav-user-info.component.scss']
})
export class NavUserInfoComponent implements OnInit {
  public loggedUser$ = new BehaviorSubject(null);
  public userProfilePic: SafeResourceUrl;

  constructor(
    private tokenStorageService: TokenStorageService,
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this._getLoggedUser();
  }

  public doSignOut = () => {
    this.tokenStorageService.removeUserSession();

    setTimeout(() => {
      // this.router.navigate(['/']);
      this.router.navigate(['/login']);
    }, 1000);
    // this.router.navigate(['/login']);
  };

  private _getLoggedUser = () => {
    const loggedUser: any = this.tokenStorageService.getUser();

    if (loggedUser && Object.keys(loggedUser).length > 0) {
      this.loggedUser$.next(loggedUser);
    }

    this.userService.GetUserProfilePicbyId(loggedUser.userId).subscribe(
      data => {
        console.log(data);
        this.userProfilePic = this.sanitizer.bypassSecurityTrustResourceUrl(data);
      },
      err => {
        console.log(err.error.message);
      },
    );

  };
}
