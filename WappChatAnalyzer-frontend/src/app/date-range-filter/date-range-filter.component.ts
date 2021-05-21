import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DateRangeType, FilterService, PeriodType } from '../services/filter.service';
import * as dateFormat from "dateformat";
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-date-range-filter',
  templateUrl: './date-range-filter.component.html',
  styleUrls: ['./date-range-filter.component.css']
})
export class DateRangeFilterComponent implements OnInit, OnDestroy {

  @ViewChild("periodButton")
  periodButton: ElementRef;

  get dateRangeType(): DateRangeType {
    return this.dateRangeTypeObject.value;
  };
  dateRangeTypes = DateRangeType;

  dateRangeTypeObject: { name: string, value: DateRangeType };
  dateRangeTypeItems = [
    {
      name: "Last",
      value: DateRangeType.LAST
    },
    {
      name: "Period",
      value: DateRangeType.PERIOD
    },
    {
      name: "Range",
      value: DateRangeType.RANGE
    },
  ]

  dateLastDaysRange: number;

  datePeriodType: PeriodType;
  datePeriodBackwardsIndex: number;

  dateRangeFrom: Date;
  dateRangeTo: Date;

  dateConfig = {
    format: "YYYY-MM-DD",
    firstDayOfWeek: "mo",
    closeOnSelectDelay: 0,
    opens: "left"
  };

  timeoutId: number = null;

  popupVisible: boolean = false;
  periodItems = [
    {
      value: PeriodType.DAY,
      name: "Today"
    },
    {
      value: PeriodType.WEEK,
      name: "This week"
    },
    {
      value: PeriodType.MONTH,
      name: "This month"
    },
    {
      value: PeriodType.YEAR,
      name: "This year"
    },
  ];

  private subscriptions: Subscription[] = [];

  constructor(private filterService: FilterService) { }

  ngOnInit(): void {
    this.getValuesFromService();
    this.subscriptions.push(this.filterService.dateFilterChanged.subscribe(() => this.getValuesFromService()));
    this.subscriptions.push(this.filterService.groupingPeriodAndDateFilterChanged.subscribe(() => this.getValuesFromService()));
  }

  ngOnDestroy() {
    while (this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  private getValuesFromService() {
    this.dateRangeTypeObject = this.dateRangeTypeItems.find(i => i.value == this.filterService.dateRangeType);
    this.dateLastDaysRange = this.filterService.dateLastDaysRange;
    this.datePeriodType = this.filterService.datePeriodType;
    this.datePeriodBackwardsIndex = this.filterService.datePeriodBackwardsIndex;
    this.dateRangeFrom = this.filterService.dateRangeFrom;
    this.dateRangeTo = this.filterService.dateRangeTo;
  }

  onDateRangeTypeChanged(value: { name: string, value: DateRangeType }) {
    this.dateRangeTypeObject = value;
    this.startApplyTimeout(1200);
  }

  onDaysRangeSelected(days: number) {
    this.dateLastDaysRange = days;
    this.startApplyTimeout(300);
  }

  onPeriodDecreaseIndex() {
    this.datePeriodBackwardsIndex++;
    this.startApplyTimeout(300);
  }

  onPeriodIncreaseIndex() {
    this.datePeriodBackwardsIndex--;
    this.startApplyTimeout(300);
  }

  onPeriodOpenPopup() {
    this.popupVisible = !this.popupVisible;
  }

  onOpenPeriodEdit(event: MouseEvent) {
    event.stopImmediatePropagation();
  }

  onPeriodEdit(event) {
    let date = event.date.toDate();

    this.datePeriodBackwardsIndex = FilterService.getBackwardsIndexForDate(date, this.datePeriodType);

    this.startApplyTimeout(0);
  }

  selectPeriod(event: MouseEvent, periodType: PeriodType) {
    event.stopImmediatePropagation();
    this.datePeriodType = periodType;
    this.datePeriodBackwardsIndex = 0;
    this.startApplyTimeout(1200);
    this.popupVisible = false;
  }

  formatPeriod() {
    var format = "dd/mm/yyyy";
    var fromToDate = this.filterService.getFromToDatesFor(this.dateRangeType, this.dateRangeFrom, this.dateRangeTo, this.dateLastDaysRange, this.datePeriodType, this.datePeriodBackwardsIndex);
    if (this.datePeriodType == PeriodType.DAY) {
      return dateFormat(fromToDate.from, format);
    }
    else if (this.datePeriodType == PeriodType.WEEK) {
      return dateFormat(fromToDate.from, "dd/mm") + " - " + dateFormat(fromToDate.to, "dd/mm/yyyy");
    }
    else if (this.datePeriodType == PeriodType.MONTH) {
      return dateFormat(fromToDate.from, "mmmm yyyy");
    }
    else if (this.datePeriodType == PeriodType.YEAR) {
      return fromToDate.from.getFullYear();
    }
  }

  @HostListener('document:mousedown', ['$event'])
  onGlobalClick(event): void {
    if (this.periodButton == null)
      return;
    if (!this.periodButton.nativeElement.contains(event.target)) {
      this.popupVisible = false;
    }
  }

  onFromDateSelected(event) {
    this.dateRangeFrom = event.date.toDate();
    this.startApplyTimeout(800);
  }

  onToDateSelected(event) {
    this.dateRangeTo = event.date.toDate();
    this.startApplyTimeout(800);
  }

  startApplyTimeout(ms: number) {
    if (this.timeoutId != null)
      clearTimeout(this.timeoutId);
    this.timeoutId = <number><unknown>setTimeout(() => {
      this.timeoutId = null;
      this.apply();
    }, ms);
  }

  apply() {
    this.filterService.dateLastDaysRange = this.dateLastDaysRange;
    this.filterService.datePeriodType = this.datePeriodType;
    this.filterService.datePeriodBackwardsIndex = this.datePeriodBackwardsIndex;
    this.filterService.dateRangeFrom = this.dateRangeFrom;
    this.filterService.dateRangeTo = this.dateRangeTo;

    if (this.dateRangeType == DateRangeType.LAST)
      this.filterService.applyDateLastRange(this.dateLastDaysRange);
    else if (this.dateRangeType == DateRangeType.PERIOD)
      this.filterService.applyDatePeriodRange(this.datePeriodType, this.datePeriodBackwardsIndex);
    else if (this.dateRangeType == DateRangeType.RANGE)
      this.filterService.applyDateRange(this.dateRangeFrom, this.dateRangeTo);
  }

}
