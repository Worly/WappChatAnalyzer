import { Component, HostListener, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { EventInfo } from "../dtos/event";
import { groupBy } from "../utils";
import * as dateFormat from "dateformat";
import { Router } from '@angular/router';
import { FilterService } from '../services/filter.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  events: { [Key: string]: EventInfo[] };

  skip = 0;
  take = 10;

  editingEventInfo: EventInfo = null;
  addingNew: boolean = false;

  count: number;

  constructor(private eventService: EventService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.filterService.eventGroupsChanged.subscribe(() => this.load());
    this.filterService.dateFilterChanged.subscribe(() => this.load());
    this.load();
  }

  load() {
    this.skip = 0;
    this.events = {};
    this.loadMore();

    this.eventService.getEventCount().subscribe(count => {
      this.count = count;
    });
  }

  loadMore() {
    this.eventService.getEvents(this.skip, this.take).subscribe(events => {
      var grouped = groupBy(events, "date");
      for (let date in grouped) {
        if (this.events.hasOwnProperty(date))
          this.events[date].push(...grouped[date]);
        else
          this.events[date] = [...grouped[date]];
      }
    });
    this.skip += this.take;
  }

  formatDate(str: string, showDay: boolean) {
    let date = new Date(str);
    var format = "dd/mm/yyyy";
    if (showDay)
      format += " - dddd";
    return dateFormat(date, format);
  }

  @HostListener("window:scroll", ["$event"])
  onWindowScroll() {
    if (document.documentElement.scrollTop + window.innerHeight >= document.documentElement.scrollHeight) {
      this.loadMore();
    }
  }

  onClick(event: EventInfo) {
    this.editingEventInfo = event;
  }

  onNewDone(e: { saved: boolean, deleted: boolean, info: EventInfo }) {
    this.addingNew = false;

    if (!e.saved)
      return;

    this.addToList(e.info);
  }

  onEditDone(e: { saved: boolean, deleted: boolean, info: EventInfo }) {

    let oldDate = this.editingEventInfo.date;
    let newDate = e.info.date;
    let temp = this.editingEventInfo;

    if (!e.saved) {
      if (e.deleted)
        this.removeFromList(temp);

      this.editingEventInfo = null;
      return;
    }

    this.editingEventInfo.id = e.info.id;
    this.editingEventInfo.name = e.info.name;
    this.editingEventInfo.emoji = e.info.emoji;
    this.editingEventInfo.groupName = e.info.groupName;
    this.editingEventInfo.date = e.info.date;
    this.editingEventInfo = null;

    setTimeout(() => {
      if (oldDate != newDate) {
        this.removeFromList(temp, oldDate);
        this.addToList(temp);
      }
    }, 500);
  }

  removeFromList(eventInfo: EventInfo, fromDate?: string) {
    if (fromDate == null)
      fromDate = eventInfo.date;

    if (this.events.hasOwnProperty(fromDate)) {
      var index = this.events[fromDate].indexOf(eventInfo);
      if (index != -1) {
        this.events[fromDate].splice(index, 1);
        if (this.events[fromDate].length == 0)
          delete this.events[fromDate];

        this.count--;
      }
    }
  }

  addToList(eventInfo: EventInfo) {
    if (this.events.hasOwnProperty(eventInfo.date))
      this.events[eventInfo.date].push(eventInfo);
    else
      this.events[eventInfo.date] = [eventInfo];

    this.count++;
  }

  onNewClick() {
    this.addingNew = true;
  }
}
