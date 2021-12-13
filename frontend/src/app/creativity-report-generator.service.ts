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

  getCreativityReportItems(startDate : string, endDate : string, userName :  string): Observable<CreativityReportItem[]> {
    let params = new HttpParams();
    params = params.append('startDate', startDate);
    params = params.append('endDate', endDate);
    params = params.append('userName', userName);
    
    return this.http.get<CreativityReportItem[]>(this.APIUrl, { params: params, responseType: 'json'} );
  }
}