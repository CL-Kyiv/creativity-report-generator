import { Component } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { Subject } from 'rxjs';
import { GridApi } from 'ag-grid-community';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private gridApi: GridApi;

  constructor(
  ) {}

  onGridReady(params: any) {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  rowData = [];

  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      editable: false,
      width: 10,
    },
    {
      headerName: 'Edit date',
      field: 'editDate',
      editable: false,
      width: 10,
    },
    {
      headerName: 'Project Name',
      field: 'projectName',
      editable: false,
      width: 10,
    },
    {
      headerName: 'Commit ID',
      field: 'commitId',
      editable: false,
      width: 30,
    },
    {
      headerName: 'Comment',
      field: 'comment',
      editable: false,
      width: 40,
    },
  ];

  isGenerate : boolean = false;

  onGenerate(){
    this.isGenerate = true;
  }
}
