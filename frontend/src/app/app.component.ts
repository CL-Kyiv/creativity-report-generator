import { Component } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { Observable } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { CreativityReportItem } from './creativity-report-item';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private gridApi: GridApi;

  constructor(private service: CreativityReportGeneratorService) {}

  onGridReady(params: any) {
    this.gridApi = params.api;
    this.gridApi.sizeColumnsToFit();
  }

  rowData$: Observable<CreativityReportItem[]>;

  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      editable: false,
      width: 10,
    },
    {
      headerName: 'End date',
      field: 'endDate',
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

  onGenerate(startDate : string, endDate : string, userName :  string){
    this.rowData$ = this.service.getCreativityReportItems(startDate, endDate, userName);
    this.isGenerate = true;
  }
}
