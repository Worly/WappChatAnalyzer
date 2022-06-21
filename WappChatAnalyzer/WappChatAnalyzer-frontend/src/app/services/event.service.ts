import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { EventInfo, Event, EventGroup, EventTemplate } from "../dtos/event";
import { FilterService } from './filter.service';
import * as dateFormat from "dateformat";

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient, private filterService: FilterService) { }

  getParams() {
    var params = {};

    params["notSelectedGroupsJSON"] = JSON.stringify(this.filterService.eventFilters.eventGroupsNotSelected);
    params["searchTerm"] = this.filterService.eventFilters.eventSearchTerm;

    var format = "yyyy-mm-dd";
    var fromToDates = this.filterService.getFromToDates();
    if (fromToDates.from != null)
      params["fromDate"] = dateFormat(fromToDates.from, format);
    if (fromToDates.to != null)
      params["toDate"] = dateFormat(fromToDates.to, format);

    return params;
  }

  getEvents(skip?: number, take?: number): Observable<EventInfo[]> {
    let params = this.getParams();
    if (skip != null)
      params["skip"] = skip;
    if (take != null)
      params["take"] = take;

    return <Observable<EventInfo[]>>this.http.get(appConfig.apiUrl + "event/getEvents", {
      params: params
    });
  }

  getEventCount(): Observable<number> {
    return <Observable<number>>this.http.get(appConfig.apiUrl + "event/getEventCount", {
      params: this.getParams()
    });
  }

  getEventGroups(): Observable<EventGroup[]> {
    return <Observable<EventGroup[]>>this.http.get(appConfig.apiUrl + "event/getEventGroups");
  }

  getEvent(id: number): Observable<Event> {
    return <Observable<Event>>this.http.get(appConfig.apiUrl + "event/getEvent/" + id);
  }

  saveEvent(event: Event): Observable<Event> {
    return <Observable<Event>>this.http.post(appConfig.apiUrl + "event/saveEvent/" + event.id, event);
  }

  deleteEvent(eventId: number): Observable<any> {
    return this.http.delete(appConfig.apiUrl + "event/deleteEvent/" + eventId);
  }

  getEventTemplates(): Observable<EventTemplate[]> {
    return this.http.get<EventTemplate[]>(appConfig.apiUrl + "event/getTemplates");
  }
}