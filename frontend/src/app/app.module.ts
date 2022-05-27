import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AgGridModule } from 'ag-grid-angular';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'
import { AppComponent } from './app.component';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ColumnAddDialogComponent } from './column-add-dialog/column-add-dialog.component';
import { CustomDateComponent } from './custom-date-component.component';
import { SelectServiceDialogComponent } from './select-service-dialog/select-service-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    ColumnAddDialogComponent,
    CustomDateComponent,
    SelectServiceDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AgGridModule.withComponents([CustomDateComponent]),
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDialogModule
  ],
  providers: [CreativityReportGeneratorService,],
  bootstrap: [AppComponent]
})
export class AppModule { }
