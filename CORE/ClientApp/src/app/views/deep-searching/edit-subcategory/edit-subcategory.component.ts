import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-edit-subcategory',
  templateUrl: './edit-subcategory.component.html',
  styleUrls: ['./edit-subcategory.component.css']
})
export class EditSubcategoryComponent implements OnInit {

  headingtext : string = "";
  isedit : boolean = false;
  constructor(public dialog:MatDialog) { }

  ngOnInit(): void {

    if(window.location.pathname == "/core/deep-search-setting/createsubcategory"){
      this.headingtext = "Create Subcategories of HSE Sharing";
      this.isedit = true;
    }
    else if(window.location.pathname == "/core/deep-search-setting/editsubcategory"){
      this.headingtext = "Edit HSE Bulletin subcategory:";
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
