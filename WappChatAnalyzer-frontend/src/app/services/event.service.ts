import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { EventInfo, Event, EventGroup } from "../dtos/event";
import { FilterService } from './filter.service';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient, private filterService: FilterService) { }

  getEvents(skip?: number, take?: number): Observable<EventInfo[]> {
    let params = {};
    if (skip != null)
      params["skip"] = skip;
    if (take != null)
      params["take"] = take;

    params["notSelectedGroupsJSON"] = JSON.stringify(this.filterService.eventGroupsNotSelected);

    return <Observable<EventInfo[]>>this.http.get(appConfig.apiUrl + "event/getEvents", {
      params: params
    });
  }

  getEventCount(): Observable<number> {
    let params = {};
    params["notSelectedGroupsJSON"] = JSON.stringify(this.filterService.eventGroupsNotSelected);
    return <Observable<number>>this.http.get(appConfig.apiUrl + "event/getEventCount", {
      params: params
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
}