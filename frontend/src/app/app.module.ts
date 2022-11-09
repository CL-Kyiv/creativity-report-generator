import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AgGridModule } from 'ag-grid-angular';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreativityReportGeneratorService } from './creativity-report-generator.service'
import { AppComponent } from './app.component';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ColumnAddDialogComponent } from './column-add-dialog/column-add-dialog.component';
import { CustomDateComponent } from './custom-date/custom-date.component';

// NG Translate
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { CustomHeaderComponent } from './custom-header/custom-header.component';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import { BitbucketComponent } from './bitbucket/bitbucket.component';
import { LocalComponent } from './local/local.component';
import { BitbucketAuthorizationDialogComponent } from './bitbucket-authorization-dialog/bitbucket-authorization-dialog.component';

// AoT requires an exported function for factories
const httpLoaderFactory = (http: HttpClient): TranslateHttpLoader => new TranslateHttpLoader(http, './assets/i18n/', '.json');

@NgModule({
  declarations: [AppComponent,
    ColumnAddDialogComponent,
    CustomDateComponent,
    CustomHeaderComponent,
    BitbucketComponent,
    LocalComponent,
    BitbucketAuthorizationDialogComponent],
  imports: [
    BrowserModule,
    AgGridModule.withComponents([CustomDateComponent]),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDialogModule,
    FontAwesomeModule,
    FormsModule,
    HttpClientModule,
    CoreModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [CreativityReportGeneratorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
