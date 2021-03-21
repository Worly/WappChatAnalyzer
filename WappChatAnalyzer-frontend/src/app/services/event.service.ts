import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class EventService {

    constructor(private http: HttpClient) { }

    getEvents(): Observable<Event[]> {
        return <Observable<Event[]>>this.http.get(appConfig.apiUrl + "event/getEvents");
    }
}