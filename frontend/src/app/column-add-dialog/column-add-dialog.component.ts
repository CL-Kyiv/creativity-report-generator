import { Component, Inject } from "@angular/core";
import { ColDef } from 'ag-grid-community';

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
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<ColumnAddDialogComponent>) {}

  isValid : boolean = true;

  onCreateColumn(headerName : string){
    this.isValid = true;

    this.data.ColumnDefs.forEach((value : ColDef) => {
      if(value.field === headerName.toLocaleLowerCase()){
        this.isValid = false;
      }
    })

    if(this.isValid)
      this.dialogRef.close({ isAdded: true, headerName : headerName});
  }

  onClose(){
    this.dialogRef.close({ isAdded: false});
  }
}