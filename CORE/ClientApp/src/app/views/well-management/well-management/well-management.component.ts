import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeletePopupComponent } from 'src/app/shared/popups/delete-popup/delete-popup.component';

@Component({
  selector: 'app-well-management',
  templateUrl: './well-management.component.html',
  styleUrls: ['./well-management.component.css']
})
export class WellManagementComponent implements OnInit {

  constructor(public dialog : MatDialog) { }

  ngOnInit(): void {
  }


  deletewellManagement() {
    const dialogRef = this.dialog.open(DeletePopupComponent, {
      width: '550px',
      data : {type : "well"}
     
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }



}
