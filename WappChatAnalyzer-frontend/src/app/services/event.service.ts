import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { Event } from "../dtos/event";

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient) { }

  getEvents(skip?: number, take?: number): Observable<Event[]> {

    let params = {};
    if (skip != null)
      params["skip"] = skip;
    if (take != null)
      params["take"] = take;

    return <Observable<Event[]>>this.http.get(appConfig.apiUrl + "event/getEvents", {
      params: params
    });
  }
}