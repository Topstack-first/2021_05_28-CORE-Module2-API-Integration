import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ENTER } from '@angular/cdk/keycodes';
import { DocumentService,DocumentDto, FileParameter } from 'src/app/util/data-service';
import * as moment from 'moment';
import { AllComponent } from './all/all.component';
import { FormBuilder, FormGroup } from "@angular/forms";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DocumentDialog } from './dialogs';
import { TokenStorageService } from '@Core/services';

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.css']
})
export class DocumentComponent implements OnInit {
  selectedDate:string[] = [];
  dates = [
    "2021-05",
    "2021-04",
    "2021-03",
    "2021-02",
    "2021-01",
    "2020-12",
    "2020-11",
  ];

  selectedCategory:string[] = [];
  categories = [
    "Agreement",
    "Audit",
    "Compliance",
    "Design",
    "Email and Letter",
  ];

  selectedDepartment:string[] = [];
  departments = [
    "Administration",
    "Business Planning",
    "Commercial",
    "Exploration",
    "Finance",
  ];

  selectedStakeholder:string[] = [];
  stakeholders = [
    "Brunei National Petroleum",
    "Integrated Technical Review",
    "Committee",
    "Internal Commercial Task",
  ];

  selectedEvent:string[] = [];
  events = [
    "Assurance Review",
    "Contractor Risk Opportunity",
    "Framing Workshop",
  ];

  selectedLocation:string[] = [];
  locations = [
    "Brunei",
    "Convention Centre, KL",
    "International - Oversea",
    "Kuala Lumpur",
  ];

  selectedWell:string[] = [];
  wells = [
    "Kelldang North East-1",
    "Kembayau East-1",
    "Kempas-1",
    "Keratau-1",
  ];

  @ViewChild('appAll') appAll: AllComponent;
  @ViewChild('appMine') appMine: AllComponent;
  @ViewChild('appApproved') appApproved: AllComponent;
  @ViewChild('appInReview') appInReview: AllComponent;
  @ViewChild('appRejected') appRejected: AllComponent;
  @ViewChild('appOCRQueue') appOCRQueue: AllComponent;
  @ViewChild('appTrash') appTrash: AllComponent;
  constructor(private dialog:MatDialog, private tokenStorageService: TokenStorageService) {  }

  ngOnInit(): void {
  }
  addDocument():void{
    let dialogRef = this.dialog.open(DocumentDialog, {
      data: { title: "Add Document",element:null}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.appAll.loadRestApiData();
    });
  }
  tabClick($event){
    if($event.index == 0)
    {
      this.appAll.loadRestApiData();
    }
    else if($event.index == 1)
    {
      this.appMine.loadRestApiData();
    }
    else if($event.index == 2)
    {
      this.appApproved.loadRestApiData();
    }
    else if($event.index == 3)
    {
      this.appInReview.loadRestApiData();
    }
    else if($event.index == 4)
    {
      this.appRejected.loadRestApiData();
    }
    else if($event.index == 5)
    {
      this.appOCRQueue.loadRestApiData();
    }
    else if($event.index == 6)
    {
      this.appTrash.loadRestApiData();
    }
  }
  applyFilter(filters:any)
  {
    this.appAll.applyFilter();
    this.appMine.applyFilter();
    this.appApproved.applyFilter();
    this.appInReview.applyFilter();
    this.appRejected.applyFilter();
    this.appTrash.applyFilter();
    filters.toggle();
  }
  filterFunc(data:DocumentDto, filter): boolean {
    const filterData = JSON.parse(filter);
    var result:boolean = true;
    var included = false;
    if(filterData.filterDate != undefined && filterData.filterDate.length > 0)
    {
      included = false;
      filterData.filterDate.forEach(one => {
        if(moment(one).year() == moment(data.docModifiedPublishDate).year() &&
        moment(one).month() == moment(data.docModifiedPublishDate).month())
        {
          included = true;
        }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterCategory != undefined && filterData.filterCategory.length > 0)
    {
      included = false;
      filterData.filterCategory.forEach(one => {
        if(one == data.categoryName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterDepartment != undefined && filterData.filterDepartment.length > 0)
    {
      included = false;
      filterData.filterDepartment.forEach(one => {
        if(one == data.departmentName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterStakeholder != undefined && filterData.filterStakeholder.length > 0)
    {
      included = false;
      filterData.filterStakeholder.forEach(one => {
        if(one == data.stakeholderName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterEvent != undefined && filterData.filterEvent.length > 0)
    {
      included = false;
      filterData.filterEvent.forEach(one => {
        if(one == data.eventName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterLocation != undefined && filterData.filterLocation.length > 0)
    {
      included = false;
      filterData.filterLocation.forEach(one => {
        if(one == data.locationName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.filterWell != undefined && filterData.filterWell.length > 0)
    {
      included = false;
      filterData.filterWell.forEach(one => {
        if(one == data.wellName) { included = true; }
      });
      if(!included) { result = false; }
    }

    if(filterData.searchStr != undefined && filterData.searchStr != "" && result == true)
    {
      filterData.searchStr = filterData.searchStr.toLocaleLowerCase();
      let searched = false;
      if(data.documentTitle != undefined &&
        data.documentTitle.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.docModifiedPublishDate != undefined &&
        data.docModifiedPublishDate.toString().toLocaleLowerCase().includes(filterData.searchStr))
      {
          searched = true;
      }
      if(data.categoryName != undefined &&
        data.categoryName.toLocaleLowerCase().toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.departmentName != undefined &&
        data.departmentName.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.stakeholderName != undefined &&
        data.stakeholderName.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.eventName != undefined &&
        data.eventName.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.locationName != undefined &&
        data.locationName.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }
      if(data.wellName != undefined &&
        data.wellName.toLocaleLowerCase().includes(filterData.searchStr))
        {
          searched = true;
        }

      if(data.userName != undefined &&
        data.userName.toLocaleLowerCase().includes(filterData.searchStr))
      {
        searched = true;
      }

      result = searched;
    }
    return result;
}
}


