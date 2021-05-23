import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, Unsubscribable } from 'rxjs';
import { FilterService, FilterType } from '../services/filter.service';

@Component({
  selector: 'app-grouping-period-picker',
  templateUrl: './grouping-period-picker.component.html',
  styleUrls: ['./grouping-period-picker.component.css']
})
export class GroupingPeriodPickerComponent implements OnInit, OnDestroy {

  groupingPeriods = [
    { value: "timeOfDay", display: "Time of day" },
    { value: "hour", display: "Hour" },
    { value: "date", display: "Date" },
    { value: "week", display: "Week" },
    { value: "month", display: "Month" },
  ];

  groupingPeriodObject: { value: string, display: string };

  timeoutId: number = null;

  private subscriptions: Unsubscribable[] = [];

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.getValuesFromService();
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.GROUPING_PERIOD], () => this.getValuesFromService()));
  }

  ngOnDestroy() {
    while (this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  private getValuesFromService() {
    this.groupingPeriodObject = this.groupingPeriods.find(i => i.value == this.filterService.groupingPeriod);
  }

  onGroupingPeriodChanged(value: { value: string, display: string }) {
    this.groupingPeriodObject = value;
    this.startApplyTimeout(500);
  }

  startApplyTimeout(ms: number) {
    if (this.timeoutId != null)
      clearTimeout(this.timeoutId);
    this.timeoutId = <number><unknown>setTimeout(() => {
      this.timeoutId = null;
      this.apply();
    }, ms);
  }

  private apply() {
    this.filterService.applyGroupingPeriod(this.groupingPeriodObject.value);
  }

}
