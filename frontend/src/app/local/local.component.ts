import { ElectronService } from '../common/core/services';
import { TranslateService } from '@ngx-translate/core';
import { APP_CONFIG } from '../../environments/environment';
import { Component, NgZone, ElementRef } from '@angular/core';
import { ColDef, IFilterDef } from 'ag-grid-community';
import { filter, Observable, map, catchError, of } from 'rxjs';
import { GridApi } from 'ag-grid-community';
import { CreativityReportItem } from '../creativity-report-item';
import { MatDialog } from '@angular/material/dialog';
import { LocalCreativityReportGeneratorService } from './local-creativity-report-generator.service';
import { ColumnAddDialogComponent } from '../common/column-add-dialog/column-add-dialog.component';
import { CustomDateComponent } from '../common/custom-date/custom-date.component';
import { FormControl, FormBuilder } from '@angular/forms';
import { CustomHeaderComponent } from '../common/custom-header/custom-header.component';
import { HeaderComponent } from '@ag-grid-community/core/dist/cjs/components/framework/componentTypes';

@Component({
  selector: 'app-local',
  templateUrl: './local.component.html',
  styleUrls: ['./local.component.css']
})

export class LocalComponent {
  public frameworkComponents;
  public defaultColDef;
  gridApi: GridApi;
  messageError : string;
  path: string | null = null;
  selectedDate = new FormControl();
  author = new FormControl();
  startWorkingHours = new FormControl();
  endWorkingHours = new FormControl();
  isSelectedPath : boolean = false;
  isMessageError : boolean = false;
  isGenerate : boolean = false;
  isHideMergeCommits : boolean = false;
  isSelectAuthorsRequestInProgress : boolean = false;
  isGitRepo : boolean = true;
  allAuthors : string[];
  rowData: CreativityReportItem[];
  mergeCommitsIds: string[];

  generateForm = this.formBuilder.group({
    selectedDate: this.selectedDate,
    author: this.author,
    startWorkingHours: this.startWorkingHours,
    endWorkingHours: this.endWorkingHours
  });

  getAuthorsForm = this.formBuilder.group({
    selectedDate: this.selectedDate
  });

  constructor(
    private electronService: ElectronService,
    private ngZone: NgZone,
    private translate: TranslateService,
    private service: LocalCreativityReportGeneratorService,
    private matDialog: MatDialog,
    private formBuilder: FormBuilder) {
    this.electronService.ipcRenderer.on('file', (event, file, isGitRepo) => {
        this.ngZone.run(() => {
          this.onReset();
          console.log(file)
          this.path = file;
          this.isGitRepo = isGitRepo;
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
        },
        resizable: true
      };
      this.frameworkComponents = { agDateInput: CustomDateComponent };
  }


  ngOnInit() {
  }

  getDirectoryPath() {
    this.electronService.ipcRenderer.send('open-file-dialog')
  }

  onReset() {
    this.allAuthors = null;
    this.author.setValue(null);
    this.isGitRepo = true;
    this.isHideMergeCommits = false;
    this.isGenerate = false;
    this.mergeCommitsIds = null;
    this.isHideMergeCommits = false;
    this.isSelectAuthorsRequestInProgress = false;
    this.isSelectedPath = false;
    this.selectedDate.setValue(null);
    this.startWorkingHours.setValue(null);
    this.endWorkingHours.setValue(null);
    this.rowData = undefined;
    this.gridApi = null;
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

  onGenerate(date : string){
    this.isGenerate = false;

    if(this.gridApi)
      this.gridApi.showLoadingOverlay();

    this.service.getCreativityReportItems(
      date, 
      this.author.value,
      this.path, 
      this.startWorkingHours.value, 
      this.endWorkingHours.value).subscribe(
        data => {
          this.rowData = data;
          if(this.gridApi)
            this.gridApi.hideOverlay()});

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
      data: {
        ColumnDefs: this.columnDefs,
      },
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
      flex : 2,
      headerComponentParams: {
        headerName: headerName,
        callback : (headerName : string) => {
          this.onDeleteColumn(headerName);
        }
      },
      headerComponentFramework : CustomHeaderComponent
    });
    this.gridApi.setColumnDefs(this.columnDefs);
  }

  onDeleteColumn(headerName : string){
    var index;
    this.columnDefs.forEach((value : ColDef) => {
      if(value.field === headerName.toLocaleLowerCase()){
        index = this.columnDefs.indexOf(value);
      }
    })
    if(index > -1){
      this.columnDefs.splice(index, 1);
      this.gridApi.setColumnDefs(this.columnDefs);
    }
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
