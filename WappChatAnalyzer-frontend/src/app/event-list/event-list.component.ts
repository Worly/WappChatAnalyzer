import { Component, HostListener, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { Event } from "../dtos/event";
import { groupBy } from "../utils";
import * as dateFormat from "dateformat";

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {

  events: { [Key: string]: Event[] };

  skip = 0;
  take = 10;

  constructor(private eventService: EventService) { }

  ngOnInit(): void {
    this.events = {};
    this.loadMore();
  }

  loadMore() {
    this.eventService.getEvents(this.skip, this.take).subscribe(events => {
      var grouped = groupBy(events, "date");
      for(let date in grouped) {
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

}
