import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeletePopupComponent } from 'src/app/shared/popups/delete-popup/delete-popup.component';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.css']
})
export class DepartmentComponent implements OnInit {

  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
  }


  deleteDepartment() {
    const dialogRef = this.dialog.open(DeletePopupComponent, {
      width: '550px',
      data : {type : "department"}
     
    });
    dialogRef.afterClosed().subscribe(result => {
    });
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
