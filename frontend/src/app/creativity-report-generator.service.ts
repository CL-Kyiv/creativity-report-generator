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

  getCreativityReportItems(
    date : string, 
    userName :  string,
    repositoryName : string, 
    consumerKey : string, 
    consumerSecretKey : string,  
    path : string,  
    startWorkingHours : string, 
    endWorkingHours : string): Observable<CreativityReportItem[]> {
    let body = new HttpParams({
      fromObject : {
        'date' : date,
        'userName' : userName,
        'repositoryName' : repositoryName,
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey,
        'path' : path,
        'startWorkingHours' : startWorkingHours,
        'endWorkingHours' : endWorkingHours
      }
    })
    
    return this.http.get<CreativityReportItem[]>(this.APIUrl, { params: body, responseType: 'json'} );
  }

  getAllRepositories(consumerKey : string, consumerSecretKey : string): Observable<string[]> {
    let body = new HttpParams({
      fromObject : {
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey
      }
    })

    return this.http.get<string[]>(this.APIUrl + '/repositories', { params: body, responseType: 'json'} );
  }

  getAllAuthors(
    path : string, 
    repositoryName : string, 
    consumerKey : string, 
    consumerSecretKey : string, 
    date : string): Observable<string[]> {
    let body = new HttpParams({
      fromObject : {
        'path' : path,
        'repositoryName' : repositoryName,
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey,
        'date' : date
      }
    })

    return this.http.get<string[]>(this.APIUrl + '/authors', { params: body, responseType: 'json'} );
  }

  getMergeCommitsByAuthorAndDate(
    date : string, 
    userName :  string, 
    repositoryName : string, 
    consumerKey : string, 
    consumerSecretKey : string,  
    path : string): Observable<string[]> {
    let body = new HttpParams({
      fromObject : {
        'date' : date,
        'userName' : userName,
        'repositoryName' : repositoryName,
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey,
        'path' : path
      }
    })
    
    return this.http.get<string[]>(this.APIUrl + '/mergeCommits', { params: body, responseType: 'json'} );
  }
}