import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AgGridModule } from 'ag-grid-angular';

import { HttpClientModule } from '@angular/common/http';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AgGridModule.withComponents([])
  ],
  providers: [CreativityReportGeneratorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
