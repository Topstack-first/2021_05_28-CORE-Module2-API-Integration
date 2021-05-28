import { SelectionModel } from '@angular/cdk/collections';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { DocumentDto } from 'src/app/util/data-service';
import { BulkDeleteDocumentDialog, BulkDeletePermanentDialog, BulkEditDialog, BulkRestoreDocumentDialog, DeleteDocumentDialog, DeletePermanentDialog,  DocumentDialog, RestoreDocumentDialog } from '../dialogs';
import { TrashService } from './trash.service';
@Component({
  selector: 'app-trash',
  templateUrl: './trash.component.html',
  styleUrls: ['./trash.component.css']
})
export class TrashComponent implements OnInit {
  @Input() filters:any;
  
  @Input() filterDate:string[];
  @Input() filterCategory:string[];
  @Input() filterDepartment:string[];
  @Input() filterStakeholder:string[];
  @Input() filterEvent:string[];
  @Input() filterLocation:string[];
  @Input() filterWell:string[];

  @Input() filterFunc:any;

  searchStr:string = "";

  bulkAction:string = "Bulk Action";
  bulkActions = [
    "Bulk Action",
    "Delete Permanently",
    "Restore"
  ];
  public selection = new SelectionModel<Element>(true, []);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public displayedColumns = ['select','title', 'author', 'department', 'stakeholder', 'event', 'location', 'category', 'well', 'date','document_date','action'];
  public dataSource: any;
  public pageSize:number = 5;
  public pageSizeOptions = [5,10,20];

  constructor(private trashService:TrashService, private dialog:MatDialog) { }

  ngOnInit(): void {
    this.loadRestApiData();
  }
  loadRestApiData():void {
    this.trashService.getRestApiData().subscribe(data=>{
      this.dataSource = new MatTableDataSource<DocumentDto>(data);
      this.dataSource.filterPredicate = this.filterFunc;
      this.dataSource.paginator = this.paginator;
      this.paginator.showFirstLastButtons = true;

      this.selection.clear();
    });
  }
  ngAfterViewInit() {
    
  }
  applyFilter()
  {
    this.dataSource.filter = JSON.stringify({
      filterDate:this.filterDate,
      filterCategory:this.filterCategory,
      filterDepartment:this.filterDepartment,
      filterStakeholder:this.filterStakeholder,
      filterEvent:this.filterEvent,
      filterLocation:this.filterLocation,
      filterWell:this.filterWell,
      searchStr:this.searchStr
    });
  }
  
  
  search(searchStr: string) {
    this.applyFilter();
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.paginator.pageSize;
    return numSelected === numRows || numSelected === this.dataSource.data.length;
  }

  masterToggle() {
    const skip = this.dataSource.paginator.pageSize * this.dataSource.paginator.pageIndex;
    const pagedData = this.dataSource.data.filter((u, i) => i >= skip)
      .filter((u, i) => i <this.dataSource.paginator.pageSize);
    
    
    if(!this.isAllSelected())
    {
      this.selection.clear();
      pagedData.forEach(row => this.selection.select(row));
    }
    else{
      this.selection.clear();
    }
  }
  pageSizeOptionChange():void{
      this.paginator._changePageSize(this.pageSize);
  }
  filtersToggle():void{
    this.filters.toggle();    
  }
  restoreDocument(element:any):void{
    let dialogRef = this.dialog.open(RestoreDocumentDialog, {
      data: element
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result != undefined && result.restored == true)
      {
        this.loadRestApiData();
      }
    });
  }
  deleteDocument(element:any):void{
    let dialogRef = this.dialog.open(DeletePermanentDialog, {
      data: element
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadRestApiData();
    });
  }
  apply():void{
    const numSelected = this.selection.selected.length;
    if(numSelected > 0 && this.bulkAction == "Restore")
    {
      let dialogRef = this.dialog.open(BulkRestoreDocumentDialog, {
        data:this.selection.selected
      });
      dialogRef.afterClosed().subscribe(result => {
        this.loadRestApiData();
      });
    }
    else if(numSelected > 0 && this.bulkAction == "Delete Permanently")
    {
      let dialogRef = this.dialog.open(BulkDeletePermanentDialog, {
        data:this.selection.selected
      });
      dialogRef.afterClosed().subscribe(result => {
        this.loadRestApiData();
      });
    }
  }
}
