import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { FilterService } from '../../services/filter.service';

@Component({
  selector: 'app-event-search',
  templateUrl: './event-search.component.html',
  styleUrls: ['./event-search.component.css']
})
export class EventSearchComponent implements OnInit, OnDestroy {

  searchTerm: string = "";

  timeoutId: number = null;
  private subscriptions: Subscription[] = [];

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.searchTerm = this.filterService.eventFilters.eventSearchTerm;
    this.subscriptions.push(this.filterService.eventSearchTermChanged.subscribe(searchTerm => {
      this.searchTerm = searchTerm;
    }));
  }

  ngOnDestroy() {
    while(this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  startApplyTimeout() {
    if (this.timeoutId != null)
      clearTimeout(this.timeoutId);
    this.timeoutId = <number><unknown>setTimeout(() => {
      this.timeoutId = null;
      this.apply();
    }, 400);
  }

  apply() {
    this.filterService.applyEventSearchTerm(this.searchTerm);
  }

  clear() {
    this.searchTerm = "";
    this.apply();
  }
}
