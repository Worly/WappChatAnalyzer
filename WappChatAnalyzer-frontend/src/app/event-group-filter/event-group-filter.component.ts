import { Component, ElementRef, HostListener, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EventGroup } from '../dtos/event';
import { EventService } from '../services/event.service';
import { FilterService } from '../services/filter.service';

@Component({
  selector: 'app-event-group-filter',
  templateUrl: './event-group-filter.component.html',
  styleUrls: ['./event-group-filter.component.css']
})
export class EventGroupFilterComponent implements OnInit, OnDestroy {

  popupVisible: boolean = false;

  eventGroups: EventGroup[];
  notSelected: number[] = [];

  isLoading: boolean = false;

  timeoutId: number = null;

  private subscriptions: Subscription[] = [];

  constructor(private elementRef: ElementRef, private eventService: EventService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.notSelected = this.filterService.eventFilters.eventGroupsNotSelected;
    this.subscriptions.push(this.filterService.eventGroupsChanged.subscribe(notSelected => {
      this.notSelected = notSelected;
    }));

    this.isLoading = true;
    this.eventService.getEventGroups().subscribe(eg => {
      this.eventGroups = eg;
      this.isLoading = false;
    });
  }

  ngOnDestroy() {
    while(this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  togglePopup() {
    this.popupVisible = !this.popupVisible;
  }

  @HostListener('document:mousedown', ['$event'])
  onGlobalClick(event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.popupVisible = false;
    }
  }

  isSelected(eventGroup: EventGroup) {
    return this.notSelected.indexOf(eventGroup.id) == -1;
  }

  toggle(eventGroup: EventGroup) {
    if (this.isSelected(eventGroup))
      this.notSelected.push(eventGroup.id);
    else
      this.notSelected.splice(this.notSelected.indexOf(eventGroup.id), 1);

    this.startApplyTimeout();
  }

  startApplyTimeout() {
    if (this.timeoutId != null)
      clearTimeout(this.timeoutId);
    this.timeoutId = <number><unknown>setTimeout(() => {
      this.timeoutId = null;
      this.apply();
    }, 800);
  }

  apply() {
    this.filterService.applyEventGroups(this.notSelected);
  }

  selectAll() {
    this.notSelected = [];
    this.startApplyTimeout();
  }

  selectNone() {
    this.notSelected = [ ...this.eventGroups.map(o => o.id) ];
    this.startApplyTimeout();
  }
}
