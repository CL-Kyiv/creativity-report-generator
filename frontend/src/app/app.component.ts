import { Component, ViewChild, ElementRef } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { filter, Observable } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { CreativityReportItem } from './creativity-report-item';
import { MatDialog } from '@angular/material/dialog';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'
import { ColumnAddDialogComponent } from './column-add-dialog.component/column-add-dialog.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private gridApi: GridApi;
  $allAuthors : Observable<string[]>;
  path : string;
  @ViewChild('path', { static: true }) MyDOMElement: ElementRef;

  constructor(private service: CreativityReportGeneratorService,
    private matDialog: MatDialog) {
  }
  

  onGridReady(params: any) {
    this.gridApi = params.api;
  }

  rowData$: Observable<CreativityReportItem[]>;

  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      editable: false,
      minWidth : 50,
      headerCheckboxSelection: true,
      flex : 1,
      checkboxSelection: true,
    },
    {
      headerName: 'End date',
      field: 'endDate',
      editable: false,
      minWidth : 50,
      flex : 1,
    },
    {
      headerName: 'Project Name',
      field: 'projectName',
      editable: false,
      minWidth : 70,
      flex : 1.5
    },
    {
      headerName: 'Commit ID',
      field: 'commitId',
      editable: false,
      minWidth : 100,
      flex : 2.5
    },
    {
      headerName: 'Comment',
      field: 'comment',
      editable: false,
      minWidth : 200,
      flex : 4  
    },
  ];

  isGenerate : boolean = false;

  onSelectPath(path : string){
    this.path = path;
    this.$allAuthors = this.service.getAllAuthors(path);
  }

  onGenerate(date : string, userName :  string){
    this.rowData$ = this.service.getCreativityReportItems(date, userName, this.path);
  
    this.isGenerate = true;
  }

  onBtnExport(){
    this.gridApi.exportDataAsCsv({onlySelected: true});
  }

  openAddDialog() {
    const dialogRef = this.matDialog.open(ColumnAddDialogComponent, {
      height: '200px',
      width: '350px',
    });
    dialogRef.afterClosed().pipe(filter(r => r.isAdded)).subscribe((r) => {
      this.onAddColumn(r.headerName);
    });
  }

  onAddColumn(headerName : string){
    this.columnDefs = this.columnDefs.concat([
      {
        headerName : headerName,
        field : headerName.toLocaleLowerCase(),
        minWidth : 50,
        editable: true,
        flex : 1
      }]);
    }
}
