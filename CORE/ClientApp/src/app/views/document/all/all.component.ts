import { SelectionModel } from '@angular/cdk/collections';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { Observable } from 'rxjs';
import { DocumentDto } from 'src/app/util/data-service';
import { BulkDeleteDocumentDialog, BulkEditDialog, DeleteDocumentDialog, DocumentDialog } from '../dialogs';
import { AllService } from './all.service';

@Component({
  selector: 'app-all',
  templateUrl: './all.component.html',
  styleUrls: ['./all.component.css']
})
export class AllComponent implements OnInit {
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
    "Delete",
    "Edit",
  ];
  public selection = new SelectionModel<DocumentDto>(true, []);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public displayedColumns = ['select','title', 'author', 'department', 'stakeholder', 'event', 'location', 'category', 'well', 'date','document_date','action'];
  public dataSource: MatTableDataSource<DocumentDto>;
  public restApiDataSource$:Observable<DocumentDto[]>;
  public pageSize:number = 5;
  public pageSizeOptions = [5,10,20];

  constructor(private allService:AllService, private dialog:MatDialog) { 
    
  }

  ngOnInit(): void {
    this.loadRestApiData();
  }
  loadRestApiData():void{
    this.allService.getRestApiData().subscribe(data=>{
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
      
    this.isAllSelected() ?
        this.selection.clear() :
        pagedData.forEach(row => this.selection.select(row));
  }

  pageSizeOptionChange():void{
      this.paginator._changePageSize(this.pageSize);
      
  }
  filtersToggle():void{
    this.filters.toggle();    
  }
  editDocument(element:any):void{
    let dialogRef = this.dialog.open(DocumentDialog, {
      data: { title: "Edit Document", element:element}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadRestApiData();
    });
  }
  deleteDocument(element:any):void{
    let dialogRef = this.dialog.open(DeleteDocumentDialog, {
      data: element
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result != undefined && result.deleted == true)
      {
        this.loadRestApiData();
      }
    });
  }
  apply():void{
    const numSelected = this.selection.selected.length;
    if(numSelected > 0 && this.bulkAction == "Edit")
    {
      let dialogRef = this.dialog.open(BulkEditDialog, {
        data:this.selection.selected
      });
      dialogRef.afterClosed().subscribe(result => {
        this.loadRestApiData();
      });
    }
    else if(numSelected > 0 && this.bulkAction == "Delete")
    {
      let dialogRef = this.dialog.open(BulkDeleteDocumentDialog, {
        data:this.selection.selected
      });
      dialogRef.afterClosed().subscribe(result => {
        this.loadRestApiData();
      });
    }
  }
}
