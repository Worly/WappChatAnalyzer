import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  eventGroupsNotSelected: number[] = [];

  eventGroupsChanged: Subject<number[]> = new Subject<number[]>();

  constructor() { }

  applyEventGroups(notSelected: number[]) {
    this.eventGroupsNotSelected = notSelected;
    this.eventGroupsChanged.next(notSelected);
  }
}
