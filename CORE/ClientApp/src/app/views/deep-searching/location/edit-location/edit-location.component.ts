import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-location',
  templateUrl: './edit-location.component.html',
  styleUrls: ['./edit-location.component.css']
})
export class EditLocationComponent implements OnInit {
  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/deep-search-setting/editlocation"){
      this.headingtext = "Edit HSE Bulletin Location:";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/createlocation"){
      this.headingtext = "Create Location:";
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
