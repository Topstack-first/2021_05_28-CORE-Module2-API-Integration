import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-department',
  templateUrl: './edit-department.component.html',
  styleUrls: ['./edit-department.component.css']
})
export class EditdepartmentComponent implements OnInit {
  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/deep-search-setting/editdepart"){
      this.headingtext = "Edit HSE Bulletin Department:";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/createdepart"){
      this.headingtext = "Create Department:";
      this.isedit = false;
    }
  }


  openMedia(){ 
    this.dialog.open(MediauploadComponent, {
      maxWidth: '700px',
      maxHeight: 'auto',
      height: 'auto',
      width: '100%',
    }); 
  }



}
