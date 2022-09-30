import { Component, ElementRef, ViewChild } from '@angular/core';
import { IHeaderAngularComp } from 'ag-grid-angular';
import { IHeaderParams } from 'ag-grid-community';

export interface ICustomHeaderParams {
  headerName : string;
  callback : any
}

@Component({
  selector: 'app-custom-header',
  templateUrl: './custom-header.component.html',
  styleUrls: ['./custom-header.component.css']
})
export class CustomHeaderComponent implements IHeaderAngularComp{
  public params!: IHeaderParams & ICustomHeaderParams;

  agInit(params: IHeaderParams & ICustomHeaderParams): void {
    this.params = params;
  }

  onDelete(){
    this.params.callback(this.params.headerName);
  }

  refresh(params: IHeaderParams) {
    return false;
  }

}
