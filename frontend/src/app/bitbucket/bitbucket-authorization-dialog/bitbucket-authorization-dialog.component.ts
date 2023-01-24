import { Component, Inject } from "@angular/core";
import { ColDef } from 'ag-grid-community';
import { BitbucketCreativityReportGeneratorService } from '../bitbucket-creativity-report-generator.service';
import { filter, Observable, map, catchError } from 'rxjs';

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
  messageAuthorizationError : string;
  isError : boolean = false;

  constructor(private dialogRef: MatDialogRef<BitbucketAuthorizationDialogComponent>,
    private service: BitbucketCreativityReportGeneratorService) {}

  onLogIn(consumerKey : string, consumerSecretKey : string){
    this.isError = true;
    this.service
      .tryAuthorization(consumerKey, consumerSecretKey)
      .pipe(
        catchError(error => {
          this.messageAuthorizationError = error.error;
          throw error.error;
        }))
      .subscribe(isAuthorized => {
        this.messageAuthorizationError = undefined;
        this.dialogRef.close({ consumerKey: consumerKey, consumerSecretKey : consumerSecretKey});
      }
    );
  }
}