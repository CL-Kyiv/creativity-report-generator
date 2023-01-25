import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreativityReportItem } from '../creativity-report-item';
 
@Injectable({
  providedIn: 'root',
})
export class BitbucketCreativityReportGeneratorService {
  
  config = require('../config/host.config.json');

  readonly APIUrl = this.config.host.endpoint + 'BitbucketCreativityReportGenerator';
  
  constructor(private http: HttpClient) {}

  getCreativityReportItems(
    date : string, 
    userName :  string,
    repositoryName : string, 
    consumerKey : string, 
    consumerSecretKey : string,  
    startWorkingHours : string, 
    endWorkingHours : string): Observable<CreativityReportItem[]> {
    let body = new HttpParams({
      fromObject : {
        'date' : date,
        'userName' : userName,
        'repositoryName' : repositoryName,
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey,
        'startWorkingHours' : startWorkingHours,
        'endWorkingHours' : endWorkingHours
      }
    })
    
    return this.http.get<CreativityReportItem[]>(this.APIUrl, { params: body, responseType: 'json'} );
  }

  tryAuthorization(consumerKey : string, consumerSecretKey : string): Observable<boolean> {
    let body = new HttpParams({
      fromObject : {
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey
      }
    })

    return this.http.get<boolean>(this.APIUrl + '/authorization', { params: body, responseType: 'json'} );
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
    repositoryName : string, 
    consumerKey : string, 
    consumerSecretKey : string, 
    date : string): Observable<string[]> {
    let body = new HttpParams({
      fromObject : {
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
    consumerSecretKey : string): Observable<string[]> {
    let body = new HttpParams({
      fromObject : {
        'date' : date,
        'userName' : userName,
        'repositoryName' : repositoryName,
        'consumerKey' : consumerKey,
        'consumerSecretKey' : consumerSecretKey
      }
    })
    
    return this.http.get<string[]>(this.APIUrl + '/mergeCommits', { params: body, responseType: 'json'} );
  }
}