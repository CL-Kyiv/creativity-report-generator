import { Component, ViewChild, ElementRef } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { filter, Observable } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { CreativityReportItem } from './creativity-report-item';
import { MatDialog } from '@angular/material/dialog';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'
import { ColumnAddDialogComponent } from './column-add-dialog.component.ts/column-add-dialog.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private gridApi: GridApi;
  $allAuthors : Observable<string[]>;
  public defaultColDef;
  path : string;
  @ViewChild('path', { static: true }) MyDOMElement: ElementRef;

  constructor(private service: CreativityReportGeneratorService,
    private matDialog: MatDialog) {
      this.defaultColDef = {
        flex: 1,
        minWidth: 50,
        editable: false,
        floatingFilter: true
      };
  }
  
  onGridReady(params: any) {
    this.gridApi = params.api;
  }

  rowData$: Observable<CreativityReportItem[]>;

  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      headerCheckboxSelection: true,
      filter: 'agDateColumnFilter',
      checkboxSelection: true
    },
    {
      headerName: 'End date',
      field: 'endDate',
      filter: 'agDateColumnFilter',
    },
    {
      headerName: 'Project Name',
      field: 'projectName',
      minWidth : 70,
      flex : 1.5
    },
    {
      headerName: 'Commit ID',
      field: 'commitId',
      filter: 'agTextColumnFilter',
      minWidth : 100,
      flex : 2.5
    },
    {
      headerName: 'Comment',
      field: 'comment',
      filter: 'agTextColumnFilter',
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

  onMergeCommitsSelectionChanged(event : any){
    let mergeCommitNodes = this.gridApi
    .getRenderedNodes().filter(n => n.data.comment.includes("Merge pull request"));
    mergeCommitNodes.forEach(n => {
      if(event.target.checked)
      n.selectThisNode(true)
      else
      n.selectThisNode(false)
    });
  }
}
