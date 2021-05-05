import { Component, OnInit } from '@angular/core';
import { FilterService } from '../services/filter.service';

@Component({
  selector: 'app-grouping-period-picker',
  templateUrl: './grouping-period-picker.component.html',
  styleUrls: ['./grouping-period-picker.component.css']
})
export class GroupingPeriodPickerComponent implements OnInit {

  groupingPeriods = [
    { value: "hour", display: "Hour" },
    { value: "date", display: "Date" },
    { value: "week", display: "Week" },
    { value: "month", display: "Month" }
  ];

  groupingPeriodObject: { value: string, display: string };
  
  timeoutId: number = null;

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.getValuesFromService();
    this.filterService.groupingPeriodChanged.subscribe(() => {
      this.getValuesFromService();
    });
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
