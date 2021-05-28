import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-event',
  templateUrl: './edit-event.component.html',
  styleUrls: ['./edit-event.component.css']
})
export class EditEventComponent implements OnInit {
  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/deep-search-setting/editevent"){
      this.headingtext = "Edit HSE Bulletin Event:";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/createevent"){
      this.headingtext = "Create Event:";
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
