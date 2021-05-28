import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-data',
  templateUrl: './edit-data.component.html',
  styleUrls: ['./edit-data.component.css']
})
export class EditDataComponent implements OnInit {

  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog:MatDialog) { }

  ngOnInit(): void {
    if(window.location.pathname == "/core/deep-search-setting/editdata"){
      this.headingtext = "Edit HSE Sharing Category:";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/createdata"){
      this.headingtext = "Create Category:";
      this.isedit = false;
    }



  }
    // open onclick media upoad box
    openMedia(){ 
      this.dialog.open(MediauploadComponent, {
        maxWidth: '700px',
        maxHeight: 'auto',
        height: 'auto',
        width: '100%',
      }); 
    }

}
