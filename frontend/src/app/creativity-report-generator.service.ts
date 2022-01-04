import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreativityReportItem } from './creativity-report-item';
 
@Injectable({
  providedIn: 'root',
})
export class CreativityReportGeneratorService {
  
  config = require('./config/host.config.json');

  readonly APIUrl = this.config.host.endpoint + 'CreativityReportGenerator';
  
  constructor(private http: HttpClient) {}

  getCreativityReportItems(date : string, userName :  string, path : string): Observable<CreativityReportItem[]> {
    let params = new HttpParams();
    params = params.append('date', date);
    params = params.append('userName', userName);
    params = params.append('path', path);
    
    return this.http.get<CreativityReportItem[]>(this.APIUrl, { params: params, responseType: 'json'} );
  }

  getAllAuthors(path : string): Observable<string[]> {
    let params = new HttpParams();
    params = params.append('path', path);

    return this.http.get<string[]>(this.APIUrl + '/authors', { params: params, responseType: 'json'} );
  }

  getMergeCommitsByAuthorAndDate(date : string, userName :  string, path : string): Observable<string[]> {
    let params = new HttpParams();
    params = params.append('date', date);
    params = params.append('userName', userName);
    params = params.append('path', path);
    
    return this.http.get<string[]>(this.APIUrl + '/mergeCommits', { params: params, responseType: 'json'} );
  }
}