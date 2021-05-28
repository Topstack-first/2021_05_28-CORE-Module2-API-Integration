import { FixedSizeVirtualScrollStrategy } from '@angular/cdk/scrolling';
import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '@Core/services';

@Component({
  selector: 'layout-sidenav',
  templateUrl: './sidenav-layout.component.html'
})
export class SidenavLayoutComponent implements OnInit {
  expandSidebar: Boolean = false;
  loggedUserRoleId: number;

  sidebarRoutes: Array<{
    roles: Array<number>,
    name: String,
    icon?: String,
    childrens?: Array<{
      roles: Array<number>,
      name: String,
      path: String
    }>
  }> = [{
    roles: [1,2,3,4],
    name: "dashboard",
    icon: "tachometer.png",
    childrens: [{
      roles: [1,2,3,4],
      name: "Home",
      path: "/home"
    }, {
      roles: [1,2,3,4],
      name: "User Searches",
      path: "/user-searches"
    }, {
      roles: [1,2,3,4],
      name: "Admin Searches",
      path: "/admin-searches"
    }]
  }, {
    roles: [1,2,3],
    name: "core",
    icon: "core.png",
    childrens: [{
      roles: [1],
      name: "Deep Search Setting",
      path: "/deep-search-setting"
    }, {
      roles: [1],
      name: "Well Management",
      path: "/well-management"
    }, {
      roles: [1],
      name: "Admin Settings",
      path: "/admin-settings"
    }, {
      roles: [1,2],
      name: "Project Tracker Setting",
      path: "/project-tracker-setting"
    }, {
      roles: [1],
      name: "CORE Health Checkup",
      path: "/core-health-checkup"
    }, {
      roles: [1,3],
      name: "Bulk Uploader",
      path: "/bulk-uploader"
    }, {
      roles: [1,2,3,4],
      name: "All Documents",
      path: "/document"
    }]
  }, {
    roles: [1],
    name: "users",
    icon: "user.png",
    childrens: [{
      roles: [1],
      name: "All Users",
      path: "/users"
    }, {
      roles: [1],
      name: "Add New",
      path: "/users/newuser"
    }]
  }, {
    roles: [1,3],
    name: "Pages",
    icon: "page.png",
    childrens: [{
      roles: [1,3],
      name: "All Pages",
      path: "/all-pages"
    }, {
      roles: [1,3],
      name: "Category",
      path: "/category-page"
    }, {
      roles: [1,3],
      name: "Add New",
      path: "/add-new-page"
    }]
  }, {
    roles: [1,3],
    name: "Component",
    icon: "component.png",
    childrens: [{
      roles: [1,3],
      name: "All Component",
      path: "/all-component"
    }]
  }
  // , {
  //   roles: [1,2,3,4],
  //   name: "Media",
  //   icon: "media.png",
  //   childrens: [{
  //     roles: [1,2,3,4],
  //     name: "Library",
  //     path: "/media-library"
  //   }, {
  //     roles: [1,2,3,4],
  //     name: "Add New",
  //     path: "/add-new-media"
  //   }]
  // }
  , {
    roles: [1,2,3,4],
    name: "Projects",
    icon: "project.png",
    childrens: [{
      roles: [1,2],
      name: "All Projects",
      path: "/all-projects"
    }
    // , {
    //   roles: [1,2,3,4],
    //   name: "New Project",
    //   path: "/add-new-project"
    // }
    , {
      roles: [1,2],
      name: "Projects Types",
      path: "/project-types"
    }
    // , {
    //   roles: [1,2,3,4],
    //   name: "Calendar",
    //   path: "/project-calendar"
    // }
    , {
      roles: [1,2],
      name: "Team",
      path: "/project-teams"
    }, {
      roles: [1],
      name: "Project Setting",
      path: "/project-settings"
    }, {
      roles: [1,2,3,4],
      name: "Dashboard",
      path: "/project-dashboard"
    }]
  }, {
    roles: [1],
    name: "Map",
    icon: "map.png",
    childrens: [{
      roles: [1],
      name: "All Map Configurations",
      path: "/map-configurations"
    }]
  }, {
    roles: [1],
    name: "Appearance",
    icon: "brush.png",
    childrens: [{
      roles: [1],
      name: "Menus",
      path: "/appearance-menus"
    }]
  }
  // , {
  //   roles: [1,2,3,4],
  //   name: "Plugins",
  //   icon: "plugin.png",
  //   childrens: [{
  //     roles: [1,2,3,4],
  //     name: "Install Plugin",
  //     path: "/install-plugin"
  //   }, {
  //     roles: [1,2,3,4],
  //     name: "Add New",
  //     path: "/add-new-plugin"
  //   }]
  // }
  , {
    roles: [1],
    name: "Settings",
    icon: "gear.png",
    childrens: [{
      roles: [1],
      name: "Backup Management",
      path: "/backup-management"
    }, {
      roles: [1],
      name: "Search Engine Index ",
      path: "/search-engine"
    }]
  }];

  toggleNavbar() {
    this.expandSidebar = !this.expandSidebar;
  }

  containsRoles(roles, userId){
    for (let role of roles){
      if(role == userId){
        return true
      }
    }
    return false
  }

  constructor(
    private tokenStorageService: TokenStorageService
    ) { }

  ngOnInit(): void {
    this.loggedUserRoleId = 1;
    this.loggedUserRoleId = this.tokenStorageService.getUser().roleId;
    // console.log('loggeduser:', this.loggedUserRoleId)
  }

}
