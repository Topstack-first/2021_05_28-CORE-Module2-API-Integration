import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeletePopupComponent } from 'src/app/shared/popups/delete-popup/delete-popup.component';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {

  }
  openMedia() {
    this.dialog.open(MediauploadComponent, {
      maxWidth: '700px',
      maxHeight: 'auto',
      height: 'auto',
      width: '100%',
    });
  }


  deleteCategory() {
    const dialogRef = this.dialog.open(DeletePopupComponent, {
      width: '550px',
      data: { type: "category" }

    });
    dialogRef.afterClosed().subscribe(result => {

    });
  }
}
