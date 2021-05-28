import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TokenStorageService } from '@Core/services';

@Component({
  selector: 'layout-home',
  templateUrl: './home-layout.component.html',
})
export class HomeLayoutComponent implements OnInit {
  constructor(
    private tokenStorageService: TokenStorageService,
    private router: Router
    ) { }

  year = new Date().getFullYear();

  logout() {
    localStorage.clear();

    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    this._getLoggedUser();
  }

  private _getLoggedUser = () => {
    const loggedUser: Object = this.tokenStorageService.getUser();
    console.log('User from Homepage:', loggedUser);
    if(Object.keys(loggedUser).length === 0){
      this.router.navigate(['login']);
    }
  };
}
