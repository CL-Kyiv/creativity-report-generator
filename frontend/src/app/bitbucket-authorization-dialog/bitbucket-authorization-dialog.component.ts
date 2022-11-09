import { Component, Inject } from "@angular/core";
import { ColDef } from 'ag-grid-community';

import {
  MAT_DIALOG_DATA,
  MatDialogRef
} from "@angular/material/dialog";

@Component({
  selector: 'app-bitbucket-authorization-dialog',
  templateUrl: './bitbucket-authorization-dialog.component.html',
  styleUrls: ['./bitbucket-authorization-dialog.component.css']
})

export class BitbucketAuthorizationDialogComponent {
  constructor(private dialogRef: MatDialogRef<BitbucketAuthorizationDialogComponent>) {}

  onLogIn(consumerKey : string, consumerSecretKey : string){
    this.dialogRef.close({ consumerKey: consumerKey, consumerSecretKey : consumerSecretKey});
  }
}