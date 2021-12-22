import { Component, Inject } from "@angular/core";


import {
  MAT_DIALOG_DATA,
  MatDialogRef
} from "@angular/material/dialog";

@Component({
  selector: 'app-column-dialog',
  templateUrl: './column-add-dialog.component.html',
  styleUrls: ['./column-add-dialog.component.css']
})
export class ColumnAddDialogComponent {
  constructor(private dialogRef: MatDialogRef<ColumnAddDialogComponent>) {}

  onCreateColumn(headerName : string){
    this.dialogRef.close({ isAdded: true, headerName : headerName});
  }

  onClose(){
    this.dialogRef.close({ isAdded: false});
  }
}