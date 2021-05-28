import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediauploadComponent } from 'src/app/shared/popups/mediaupload/mediaupload.component';

@Component({
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.css']
})
export class AdminSettingsComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
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
