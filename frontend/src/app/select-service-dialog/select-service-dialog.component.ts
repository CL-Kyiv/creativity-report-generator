import { Component, OnInit } from '@angular/core';

import {
  MAT_DIALOG_DATA,
  MatDialogRef
} from "@angular/material/dialog";

@Component({
  selector: 'app-select-service-dialog',
  templateUrl: './select-service-dialog.component.html',
  styleUrls: ['./select-service-dialog.component.css']
})
export class SelectServiceDialogComponent{

  constructor(private dialogRef: MatDialogRef<SelectServiceDialogComponent>) { }

  onSelectBitbucketService(){
    this.dialogRef.close({ isSelectedBitbucket: true});
  }

  onSelectLocalService(){
    this.dialogRef.close({ isSelectedBitbucket: false});
  }
}
