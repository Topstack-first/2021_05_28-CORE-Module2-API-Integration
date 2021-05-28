import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-add-details',
  templateUrl: './add-details.component.html',
  styleUrls: ['./add-details.component.css']
})
export class AddDetailsComponent implements OnInit {
  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/well-management/edit-details"){
      this.headingtext = "Update Well";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/well-management/add-details"){
      this.headingtext = "Add New Well";
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
