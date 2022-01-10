import { Component, ViewChild, ElementRef } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { filter, Observable, map } from 'rxjs';
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
  gridApi: GridApi;
  allAuthors$ : Observable<string[]>;
  path : string;
  rowData$: Observable<CreativityReportItem[]>;
  rowData: CreativityReportItem[];
  isGenerate : boolean = false;
  isHideMergeCommits : boolean = false;
  mergeCommitsIds: string[];
  public defaultColDef;

  constructor(private service: CreativityReportGeneratorService,
    private matDialog: MatDialog) {
      this.defaultColDef = {
        flex: 1,
        minWidth: 50,
        editable: false,
        filter: true,
        floatingFilter: true,
        cellStyle: {
          'height': '100%',
          'display': 'flex ',
          'align-items': 'center ',
        }  
      };
  }
  
  onGridReady(params: any) {
    this.gridApi = params.api;
  }

  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      headerCheckboxSelection: true,
      filter: 'agDateColumnFilter',
      checkboxSelection: true,
      minWidth : 60,
      flex : 1.25
    },
    {
      headerName: 'End date',
      field: 'endDate',
      filter: 'agDateColumnFilter',
      minWidth : 60,
      flex : 1.25
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
    {
      headerName : "Hours",
      field : "hours",
      minWidth : 50,
      editable: true,
      flex : 1
    },
  ];

  onSelectPath(path : string){
    this.path = path;
    this.allAuthors$ = this.service.getAllAuthors(path);
  }

  onGenerate(date : string, userName :  string){
    this.service.getCreativityReportItems(date, userName, this.path).subscribe(data => this.rowData = data);
    this.service.getMergeCommitsByAuthorAndDate(date, userName, this.path).subscribe(ids => {
      this.mergeCommitsIds = ids;
    });
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

  onHideMergeCommits(event : any){
    if(event.target.checked)
      this.isHideMergeCommits = true;
    else
      this.isHideMergeCommits = false;
    this.gridApi.onFilterChanged();
  }
  
  isExternalFilterPresent() {
    return true;
  }

  doesExternalFilterPass = (node : any) => {
    if(this.isHideMergeCommits){
      return !this.mergeCommitsIds.includes(node.data.commitId);
    }
    else{
      return true;
    }
  }

  // setMergeCommitsFilter(){
  //   var commitIdFilterComponent = this.gridApi.getFilterInstance('commitId');
  //   var commitIdsWithoutMergeIds = this.rowData.map(row => row['commitId'])
  //      .filter((value) => {
  //         return !this.mergeCommitsIds.includes(value);
  //     });
  //     commitIdFilterComponent?.setModel({ values: commitIdsWithoutMergeIds });
  //   this.gridApi.onFilterChanged(); 
  // }
  
}
