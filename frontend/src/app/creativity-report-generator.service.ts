import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreativityReportItem } from './creativity-report-item';
import { Author } from './author-type';
 
@Injectable({
  providedIn: 'root',
})
export class CreativityReportGeneratorService {
  
  config = require('./config/host.config.json');

  readonly APIUrl = this.config.host.endpoint + 'CreativityReportGenerator';
  
  constructor(private http: HttpClient) {}

  getCreativityReportItems(date : string, userName :  string, path : string,  startWorkingHours : string, endWorkingHours : string): Observable<CreativityReportItem[]> {
    let params = new HttpParams();
    params = params.append('date', date);
    params = params.append('userName', userName);
    params = params.append('path', path);
    params = params.append('startWorkingHours', startWorkingHours);
    params = params.append('endWorkingHours', endWorkingHours);
    
    return this.http.get<CreativityReportItem[]>(this.APIUrl, { params: params, responseType: 'json'} );
  }

  getAllAuthors(path : string): Observable<Author[]> {
    let params = new HttpParams();
    params = params.append('path', path);

    return this.http.get<Author[]>(this.APIUrl + '/authors', { params: params, responseType: 'json'} );
  }

  getMergeCommitsByAuthorAndDate(date : string, userName :  string, path : string): Observable<string[]> {
    let params = new HttpParams();
    params = params.append('date', date);
    params = params.append('userName', userName);
    params = params.append('path', path);
    
    return this.http.get<string[]>(this.APIUrl + '/mergeCommits', { params: params, responseType: 'json'} );
  }
}