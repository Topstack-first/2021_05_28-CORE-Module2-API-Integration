import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ENTER } from '@angular/cdk/keycodes';
import {DocumentService, DocumentDto, FileParameter, UserDto} from 'src/app/util/data-service';
import * as moment from 'moment';
import { FormBuilder, FormGroup } from "@angular/forms";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { TokenStorageService } from '@Core/services'
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'document-dialog',
    templateUrl: 'document-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class DocumentDialog {
    title:string = "";
    submitButtonText = "";

    documentTitle:string;
    description:string;
    noApprovalNeeded:boolean;
    webAddress:string;
    documentContent:string;
    isAbleToExtract:boolean = true;
    isSubmitted = false;
    selectedDate:Date;
    notifyUser:string;
    additonalTag:string;
    approvalTag:string;
    userId:string;

    selectedApprovalStatus:string;
    statuses = [
      "Approved",
      "In Review",
      "Rejected",
    ];


    permissions = [
      {sub:false, name:'Private (editor only)'},
      {sub:false, name:'All Departments'},
      {sub:true, name:'Administrators'},
      {sub:true, name:'Finance'},
    ];

    selectedCategory:string;
    categories = [
      "Agreement",
      "Audit",
      "Compliance",
      "Design",
      "Email and Letter",
    ];

    selectedSubcategory:string;
    subcategories = [
      "Agreement",
      "Audit",
      "Compliance",
      "Design",
      "Email and Letter",
    ];

    selectedDepartment:string;
    departments = [
      "Administration",
      "Business Planning",
      "Commercial",
      "Exploration",
      "Finance",
    ];

    selectedStakeholder:string;
    stakeholders = [
      "Brunei National Petroleum",
      "Integrated Technical Review",
      "Committee",
      "Internal Commercial Task",
    ];

    selectedEvent:string;
    events = [
      "Assurance Review",
      "Contractor Risk Opportunity",
      "Framing Workshop",
    ];

    selectedLocation:string;
    locations = [
      "Brunei",
      "Convention Centre, KL",
      "International - Oversea",
      "Kuala Lumpur",
    ];

    selectedWell:string;
    wells = [
      "Kelldang North East-1",
      "Kembayau East-1",
      "Kempas-1",
      "Keratau-1",
    ];

    @ViewChild("fileupload", { static: false }) fileUpload: ElementRef;

    file:File;
    fileName:string;
    constructor(
      public dialogRef: MatDialogRef<DocumentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any,
      private documentService:DocumentService,
      public fb: FormBuilder,
      private http:HttpClient,
      private dialog:MatDialog,
      private toastrService: ToastrService,
      private tokenStorageService: TokenStorageService) {
        this.title  = data.title;

        if(data.element != null) {
          this.documentTitle = data.element.title;
          this.description = "Document for the "+data.element.title;

          let documentDto:DocumentDto = this.data.element;

          this.documentTitle = documentDto.documentTitle,
          this.webAddress = documentDto.documentPath,
          //this.selectedDate = documentDto.documentDate == undefined ? null:documentDto.documentDate.toDate(),
          this.selectedWell = documentDto.wellName,
          this.selectedCategory = documentDto.categoryName,
          this.selectedSubcategory = documentDto.subcategoryName,
          this.selectedDepartment = documentDto.departmentName ,
          this.selectedStakeholder = documentDto.stakeholderName ,
          this.selectedEvent = documentDto.eventName,
          this.selectedLocation = documentDto.locationName,
          this.description = documentDto.documentDescription,
          this.documentContent = documentDto.documentCustomContent,
          this.selectedApprovalStatus = documentDto.documentApproval ,
          this.additonalTag = documentDto.tagName;
        }

        if(this.title == "Edit Document") {
          this.submitButtonText = "Update Document";
        }
        else if(this.title == "Add Document"){
            this.submitButtonText = "Add Document";
        }

        const localUser = this.tokenStorageService.getUser();
        this.userId = localUser.userId;

        // Patch form value
        if (this.data.element) {
          const { element } = this.data;

          this.selectedCategory = element.categoryIndex;
          this.selectedSubcategory = element.subcategoryIndex;
          this.selectedDepartment = element.departmentIndex;
          this.selectedStakeholder = element.stakeholderIndex;
          this.selectedEvent = element.eventIndex;
          this.selectedLocation = element.locationIndex;
          this.selectedWell = element.wellIndex;
        }
    }

    submit():void {
      this.isSubmitted = true;

      if (!this.documentTitle) {
        this.toastrService.error(`<strong>Title</strong> field is required`);
        return;
      }

      if (!this.webAddress) {
        this.toastrService.error(`<strong>Web Address</strong> field is required`);
        return;
      }

      if(this.title == "" ||this.title == null || this.webAddress == "" || this.webAddress == null)
      {
        return;
      }
      if(this.title == "Edit Document")
      {
        let documentDto:DocumentDto = this.data.element;

        documentDto.documentTitle = this.documentTitle;
        documentDto.documentPath = this.webAddress;
        documentDto.documentDate = moment(this.selectedDate);
        documentDto.wellName = this.selectedWell;
        documentDto.categoryName = this.selectedCategory;
        documentDto.subcategoryName = this.selectedSubcategory;
        documentDto.departmentName = this.selectedDepartment;
        documentDto.stakeholderName = this.selectedStakeholder;
        documentDto.eventName = this.selectedEvent;
        documentDto.locationName = this.selectedLocation;
        documentDto.documentDescription = this.description;
        documentDto.documentCustomContent = this.documentContent;
        documentDto.documentApproval = this.selectedApprovalStatus;
        documentDto.tagName = this.additonalTag;

        this.documentService.updateDocument(documentDto).subscribe(result=>{
          if(this.file != null)
          {
            let formData = new FormData();

            formData.append("uploadFile", this.file);
            formData.append("uploadedFilename",""+this.fileName);
            formData.append("documentId",""+ result.documentId);
            fetch(this.documentService.getBaseUrl("")+'/api/Document/UploadDocument', {method: "POST", body: formData});
          }

          this.close();
        });
      }
      else if(this.title == "Add Document"){
        const documentDto:DocumentDto = {
          documentAuthor: this.userId,
          documentTitle: this.documentTitle,
          documentPath: this.webAddress,
          documentDate: moment(this.selectedDate),
          wellName: this.selectedWell,
          categoryName: this.selectedCategory,
          subcategoryName: this.selectedSubcategory,
          departmentName: this.selectedDepartment,
          stakeholderName: this.selectedStakeholder,
          eventName: this.selectedEvent,
          locationName: this.selectedLocation,
          documentDescription: this.description,
          documentCustomContent: this.documentContent,
          documentApproval: this.selectedApprovalStatus,
          tagName: this.additonalTag,
          deleted: 0
        };

        this.documentService.addDocument(documentDto).subscribe(result=>{
          if(this.file != null)
          {
            let formData = new FormData();

            formData.append("uploadFile", this.file);
            formData.append("uploadedFilename",""+this.fileName);
            formData.append("documentId",""+ result.documentId);
            fetch(this.documentService.getBaseUrl("")+'/api/Document/UploadDocument', {method: "POST", body: formData});
          }
          this.close();
        });

      }

    }
    close(): void {
      this.dialogRef.close();
    }
    public handleFileInput(event: any) {
      this.webAddress = "";
      this.fileName = "";

      this.isAbleToExtract = true;
      console.log(event.target.files[0]);
      this.file = event.target.files[0];
      this.fileName = this.file.name;


      this.documentService.getWebAddressForFile(this.fileName).subscribe(result=>
        {
          this.webAddress = result;
        });

      this.documentService
        .getExtractedContent(this.fileName, this.file)
        .pipe(
          tap((result: any) => {
            this.selectedCategory = result.category;
            this.selectedSubcategory = result.subCategory;
            this.selectedDepartment = result.department;
            this.selectedStakeholder = result.stakeholder;
            this.selectedEvent = result.event;
            this.selectedLocation = result.location;
            this.selectedWell = result.well;
            this.isAbleToExtract = result.isAbleToExtract;
            this.documentTitle = this.fileName;
          })
        )
        .subscribe();

        if(this.fileName.length > 70)
        {
          this.fileName = this.fileName.substr(0, 70) + "....";
        }
    }

    selectFile():void{
      this.webAddress = "";
      this.fileName = "";

      this.isAbleToExtract = true;

      let dialogRef = this.dialog.open(SelectFileDialog, {
      });

      dialogRef.afterClosed().subscribe(result => {
        this.webAddress = result.webAddress;
        this.fileName = result.webAddress.substr(result.webAddress.lastIndexOf("\\")+1);
        console.log(this.fileName);

        this.documentService
          .getExtractedContentFromExistingNetworkFile(result.webAddress)
          .pipe(
            tap((result: any) => {
              this.selectedCategory = result.category;
              this.selectedSubcategory = result.subCategory;
              this.selectedDepartment = result.department;
              this.selectedStakeholder = result.stakeholder;
              this.selectedEvent = result.event;
              this.selectedLocation = result.location;
              this.selectedWell = result.well;
              this.isAbleToExtract = result.isAbleToExtract;
              this.documentTitle = this.fileName;
            })
          )
          .subscribe();

        if(this.fileName.length > 70)
        {
          this.fileName = this.fileName.substr(0, 70) + "....";
        }
      });
    }
  }



  @Component({
    selector: 'delete-document-dialog',
    templateUrl: 'delete-document-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class DeleteDocumentDialog {
    title:string;
    author:string;
    department:string;
    stakeholder:string;

    constructor(
      public dialogRef: MatDialogRef<DeleteDocumentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto,
      private documentService:DocumentService) {
        this.title = data.documentTitle;
        this.author = data.userName;
        data.departmentName = (data.departmentName == "NotAvailable") ? "" : data.departmentName;
        data.stakeholderName = (data.stakeholderName == "NotAvailable") ? "" : data.stakeholderName;
        this.department = data.departmentName;
        this.stakeholder = data.stakeholderName;
    }
    delete():void{
      this.documentService.deleteDocumentNormal(this.data).subscribe(result=>{
        this.dialogRef.close({deleted:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }


  @Component({
    selector: 'bulk-delete-document-dialog',
    templateUrl: 'bulk-delete-document-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class BulkDeleteDocumentDialog {

    constructor(
      public dialogRef: MatDialogRef<BulkDeleteDocumentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto[],
      private documentService:DocumentService) {
    }
    delete():void{
      this.documentService.bulkDeleteDocuments(this.data).subscribe(result=>{
        this.dialogRef.close({deleted:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }

  @Component({
    selector: 'restore-document-dialog',
    templateUrl: 'restore-document-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class RestoreDocumentDialog {
    title:string;
    author:string;
    department:string;
    stakeholder:string;

    constructor(
      public dialogRef: MatDialogRef<RestoreDocumentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto,
      private documentService:DocumentService) {
        this.title = data.documentTitle;
        this.author = data.userName;
        data.departmentName = (data.departmentName == "NotAvailable") ? "" : data.departmentName;
        data.stakeholderName = (data.stakeholderName == "NotAvailable") ? "" : data.stakeholderName;
        this.department = data.departmentName;
        this.stakeholder = data.stakeholderName;
    }
    restore():void{
      this.documentService.restoreDocument(this.data).subscribe(result=>{
        this.dialogRef.close({restored:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }


  @Component({
    selector: 'bulk-restore-document-dialog',
    templateUrl: 'bulk-restore-document-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class BulkRestoreDocumentDialog {

    constructor(
      public dialogRef: MatDialogRef<BulkRestoreDocumentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto[],
      private documentService:DocumentService) {
    }
    restore():void{
      this.documentService.bulkRestoreDocuments(this.data).subscribe(result=>{
        this.dialogRef.close({restored:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }


  @Component({
    selector: 'delete-permanent-dialog',
    templateUrl: 'delete-permanent-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class DeletePermanentDialog {
    title:string;
    author:string;
    department:string;
    stakeholder:string;

    constructor(
      public dialogRef: MatDialogRef<DeletePermanentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto,
      private documentService:DocumentService) {
        this.title = data.documentTitle;
        this.author = data.userName;
        data.departmentName = (data.departmentName == "NotAvailable") ? "" : data.departmentName;
        data.stakeholderName = (data.stakeholderName == "NotAvailable") ? "" : data.stakeholderName;

        this.department = data.departmentName;
        this.stakeholder = data.stakeholderName;
    }
    deletePermanently():void{
      this.documentService.deleteDocumentPermanently(this.data).subscribe(result=>{
        this.dialogRef.close({deleted:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }


  @Component({
    selector: 'bulk-delete-permanent-dialog',
    templateUrl: 'bulk-delete-permanent-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class BulkDeletePermanentDialog {
    title:string;
    author:string;
    department:string;
    stakeholder:string;

    constructor(
      public dialogRef: MatDialogRef<BulkDeletePermanentDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DocumentDto[],
      private documentService:DocumentService) {
    }
    deletePermanently():void{
      this.documentService.bulkDeleteDocumentsPermanently(this.data).subscribe(result=>{
        this.dialogRef.close({deleted:true});
      });

    }
    close(): void {
      this.dialogRef.close();
    }
  }


  @Component({
    selector: 'bulk-edit-dialog',
    templateUrl: 'bulk-edit-dialog.html',
    styleUrls: ['./document.component.css']
  })
  export class BulkEditDialog {
    separatorKeysCodes = [ENTER];
    elements:any;
    tagName:string = "";
    userId:string;

    selectedAuthor:UserDto = null;
    authors = Array<any>();
    selectedStatus:string = null;
    statuses = [
      "Approved",
      "In Review",
      "Rejected",
    ]

    departments = [
      {checked:false, name:'Administrator'},
      {checked:false, name:'Business Planning'},
      {checked:false, name:'Commercial'},
      {checked:false, name:'Exploration'},
    ]

    stakeholders = [
      {checked:false, name:'Brunei National Petroleum'},
      {checked:false, name:'Integrated Technical Review'},
      {checked:false, name:'Internal Commercial Task Force'},
      {checked:false, name:'Joint management Committee'},
    ]

    events = [
      {checked:false, name:'Assurance Review'},
      {checked:false, name:'Contractors Risk Assessment'},
      {checked:false, name:'Meeting'},
      {checked:false, name:'Opportunity Farming Wrokshop'},
    ]

    locations = [
      {checked:false, name:'Brunei'},
      {checked:false, name:'Convention Centre ,KL'},
      {checked:false, name:'International - Oversea'},
      {checked:false, name:'Kuala Lumpur'},
    ]

    wells = [
      {checked:false, name:'Kelidang North East - 1'},
      {checked:false, name:'Kembayau East - 1'},
      {checked:false, name:'Kempas - 1'},
      {checked:false, name:'Keratau - 1'},
    ]

    categories=[
      {checked:false,name:'Agreement', subcategories:[
        {checked:false,name:'Confidentially Agreement'},
        {checked:false,name:'DEED'},
        {checked:false,name:'Framework Agreement'},
      ]},
      {checked:false,name:'Agreement2', subcategories:[
        {checked:false,name:'Confidentially Agreement'},
        {checked:false,name:'DEED'},
        {checked:false,name:'Framework Agreement'},
      ]}
    ]

    constructor(
      public dialogRef: MatDialogRef<BulkEditDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any,
      private documentService:DocumentService,
      private tokenStorageService: TokenStorageService) {
        this.elements = data;
        const localUser = this.tokenStorageService.getUser();
        this.loadAllUsers();
        this.userId = localUser.userId;
    }
    update():void {
      let tagName = this.tagName;
      let approvalStatus = this.selectedStatus;
      let author = this.selectedAuthor;
      let departmentName = "";
      this.departments.forEach(element => {
        if(element.checked == true)
        {
          departmentName = element.name;
        }
      });
      let stakeholder = "";
      this.stakeholders.forEach(element => {
        if(element.checked == true)
        {
          stakeholder = element.name;
        }
      });
      let event = "";
      this.events.forEach(element => {
        if(element.checked == true)
        {
          event = element.name;
        }
      });
      let location = "";
      this.locations.forEach(element => {
        if(element.checked == true)
        {
          location = element.name;
        }
      });
      let well = "";console.log(this.wells);
      this.wells.forEach(element => {
        if(element.checked == true)
        {
          well = element.name;
        }
      });
      let category = "";
      let subcategory = "";
      this.categories.forEach(element => {
        if(element.checked == true)
        {
          category = element.name;
        }
        element.subcategories.forEach(element => {
          if(element.checked == true)
          {
            subcategory = element.name;
          }

        });
      });
      this.data.forEach(doc=>{
        doc.tagName =  (tagName == null || tagName == "")?doc.tagName:tagName;
        doc.documentApproval =   (approvalStatus == null || approvalStatus == "")?doc.documentApproval:approvalStatus;
        doc.documentAuthor =  (author == null || author == "")?doc.documentAuthor:author;
        doc.departmentName =  (departmentName == null || departmentName == "")?doc.departmentName:departmentName;
        doc.stakeholderName = (stakeholder == null || stakeholder == "")?doc.stakeholderName:stakeholder;
        doc.eventName =  (event == null || event == "")?doc.eventName:event;
        doc.locationName =  (location == null || location == "")?doc.locationName:location;
        doc.wellName =  (well == null || well == "")?doc.wellName:well;
        doc.categoryName =  (category == null || category == "")?doc.categoryName:category;
        doc.subcategoryName =  (subcategory == null || subcategory == "")?doc.subcategoryName:subcategory;
      });
      this.documentService.bulkUpdateDocuments(this.data).subscribe(result=>{
        this.close();
      });

    }
    close(): void {
      this.dialogRef.close();
    }
    remove(element: any): void {
      let index = this.elements.indexOf(element);

      if (index >= 0) {
        this.elements.splice(index, 1);
      }
    }
    loadAllUsers():void{
      this.documentService.getAllUsers().subscribe(users=>{
        users.forEach(user=> {
          this.authors.push(user);
        });

      });
    }
  }



@Component({
  selector: 'select-file-dialog',
  templateUrl: 'select-file-dialog.html',
  styleUrls: ['./document.component.css']
})
export class SelectFileDialog {
  networkPath:string = "";
  constructor(
    public dialogRef: MatDialogRef<SelectFileDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private documentService:DocumentService) {

  }
  submit():void{
    this.documentService.getWebAddressForFile(this.networkPath).subscribe(result=>
      {
        this.dialogRef.close({
          webAddress:result
        });
      });
  }
  close(): void {
    this.dialogRef.close();
  }
}

