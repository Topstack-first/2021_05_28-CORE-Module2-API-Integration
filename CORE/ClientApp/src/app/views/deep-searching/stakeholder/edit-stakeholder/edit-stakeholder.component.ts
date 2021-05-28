import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-stakeholder',
  templateUrl: './edit-stakeholder.component.html',
  styleUrls: ['./edit-stakeholder.component.css']
})
export class EditStakeholderComponent implements OnInit {
  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/deep-search-setting/editstakeholder"){
      this.headingtext = "Edit HSE Bulletin stakeHolder:";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/createstakeholder"){
      this.headingtext = "Create Stakeholder:";
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
