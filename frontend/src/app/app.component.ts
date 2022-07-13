import { ElectronService } from './core/services';
import { TranslateService } from '@ngx-translate/core';
import { APP_CONFIG } from '../environments/environment';
import { Component, NgZone, ElementRef } from '@angular/core';
import { ColDef, IFilterDef } from 'ag-grid-community';
import { filter, Observable, map, catchError, of } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { CreativityReportItem } from './creativity-report-item';
import { MatDialog } from '@angular/material/dialog';
import { CreativityReportGeneratorService } from './creativity-report-generator.service';
import { ColumnAddDialogComponent } from './column-add-dialog/column-add-dialog.component';
import { Author } from './author-type';
import { CustomDateComponent } from './custom-date-component.component';
import { FormControl, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  path: string | null = null;

  gridApi: GridApi;
  allAuthors : string[];
  rowData: CreativityReportItem[];
  isGenerate : boolean = false;
  isHideMergeCommits : boolean = false;
  mergeCommitsIds: string[];
  public frameworkComponents;
  public defaultColDef;
  selectedDate = new FormControl();
  messageError : string;
  isSelectAuthorsRequestInProgress : boolean = false;
  author = new FormControl();
  isSelectedPath : boolean = false;
  isMessageError : boolean = false;

  generateForm = this.formBuilder.group({
    path: '',
    selectedDate: this.selectedDate,
    author: this.author,
    start: '',
    end: ''
  });

  getAuthorsForm = this.formBuilder.group({
    path: '',
    selectedDate: this.selectedDate
  });

  constructor(
    private electronService: ElectronService,
    private ngZone: NgZone,
    private translate: TranslateService,
    private service: CreativityReportGeneratorService,
    private matDialog: MatDialog,
    private formBuilder: FormBuilder) {
    this.electronService.ipcRenderer.on('file', (event, file) => {
        this.ngZone.run(() => {
          this.path = file;
        })
      })
    this.translate.setDefaultLang('en');
      this.defaultColDef = {
        flex: 1,
        minWidth: 50,
        editable: false,
        floatingFilter: true,
        cellStyle: {
          'height': '100%',
          'display': 'flex ',
          'align-items': 'center ',
        }  
      };
      this.frameworkComponents = { agDateInput: CustomDateComponent };
  }


  ngOnInit() {
  }
  getDirectoryPath() {
    this.electronService.ipcRenderer.send('open-file-dialog')
  }

  ngOnDestroy() {
    this.electronService.ipcRenderer.removeAllListeners('file')
  }
  
  onGridReady(params: any) {
    this.gridApi = params.api;
  }

  filterParams = {
    value : this.selectedDate,
    onChange : this.selectedDate,
    comparator: (filterLocalDateAtMidnight : any, cellValue : any) => {
      const dateAsString = cellValue;
      const dateParts = dateAsString.split('-');
      const cellDate = new Date(
        Number(dateParts[0]),
        Number(dateParts[1]) - 1,
        Number(dateParts[2])
      );
      if (filterLocalDateAtMidnight.getTime() === cellDate.getTime()) {
        return 0;
      }
      if (cellDate < filterLocalDateAtMidnight) {
        return -1;
      }
      if (cellDate > filterLocalDateAtMidnight) {
        return 1;
      }
      return 0;
    }
  };


  columnDefs: ColDef[] = [
    {
      headerName: 'Start date',
      field: 'startDate',
      headerCheckboxSelection: true,
      filterParams : this.filterParams,
      filter: 'agDateColumnFilter',
      checkboxSelection: true,
      minWidth : 60,
      flex : 1.25
    },
    {
      headerName: 'End date',
      field: 'endDate',
      filter: 'agDateColumnFilter',
      filterParams : this.filterParams,
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

  onSelectPath(date : string){
    this.isSelectedPath = true;
    this.isSelectAuthorsRequestInProgress = true;
    this.service
      .getAllAuthors(this.path, date)
      .pipe(
        catchError(error => {
          this.messageError = error.error;
          this.isSelectedPath = false;
          this.isSelectAuthorsRequestInProgress = false;
          return of([])
        })
      ).subscribe(authors => {
        this.allAuthors = authors;
        this.isSelectAuthorsRequestInProgress = false; 
      });
  }

  onGenerate(date : string, startWorkingHours : string, endWorkingHours : string){
    this.service.getCreativityReportItems(date, this.author.value, this.path, startWorkingHours, endWorkingHours).subscribe(data => this.rowData = data);

    this.service.getMergeCommitsByAuthorAndDate(date, this.author.value, this.path).subscribe(ids => {
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
    this.columnDefs.push(
    {
      headerName : headerName,
      field : headerName.toLocaleLowerCase(),
      minWidth : 50,
      editable: true,
      flex : 1
    });
    this.gridApi.setColumnDefs(this.columnDefs);
  }

  onHideMergeCommits(event : any){
    this.isHideMergeCommits = event.target.checked;
    this.gridApi.onFilterChanged();
  }
  
  isExternalFilterPresent = () => {
    return this.isHideMergeCommits;
  }

  doesExternalFilterPass = (node : any) => {
    return !this.mergeCommitsIds.includes(node.data.commitId);
  }
}
