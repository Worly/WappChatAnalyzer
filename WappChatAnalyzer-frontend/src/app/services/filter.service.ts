import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, Unsubscribable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  eventGroupsNotSelected: number[] = [];
  eventGroupsChanged: Subject<number[]> = new Subject<number[]>();

  eventSearchTerm: string = "";
  eventSearchTermChanged: Subject<string> = new Subject<string>();


  dateRangeType: DateRangeType = DateRangeType.LAST;

  dateLastDaysRange: number = 30;

  datePeriodType: PeriodType = PeriodType.MONTH;
  datePeriodBackwardsIndex: number = 0;

  dateRangeFrom: Date;
  dateRangeTo: Date;

  groupingPeriod: string = "date";

  private dateAndGroupingHistory: {
    dateRangeType: DateRangeType,
    dateLastDaysRange: number,
    datePeriodType: PeriodType,
    datePeriodBackwardsIndex: number,
    dateRangeFrom: Date,
    dateRangeTo: Date,
    groupingPeriod: string
  }[] = [];

  private filterChangedSubscribers: { subscribedTo: FilterType[], callback: () => void }[] = [];

  constructor() {
    this.dateRangeTo = new Date();

    var today = new Date();
    today.setDate(today.getDate() - 30);
    this.dateRangeFrom = today;
  }

  subscribeToFilterChanged(filterTypes: FilterType[], callback: () => void): Unsubscribable {
    var subscription = {
      subscribedTo: filterTypes,
      callback: callback
    };
    this.filterChangedSubscribers.push(subscription);

    return {
      unsubscribe: () => {
        let index = this.filterChangedSubscribers.indexOf(subscription);
        if (index != -1)
          this.filterChangedSubscribers.splice(index, 1);
      }
    }
  }

  private emitFilterChanged(filterTypes: FilterType[]) {
    for (let subscriber of this.filterChangedSubscribers) {
      if (filterTypes.some(o => subscriber.subscribedTo.includes(o)))
        subscriber.callback();
    }
  }

  private saveHistory() {
    this.dateAndGroupingHistory.push({
      dateLastDaysRange: this.dateLastDaysRange,
      datePeriodBackwardsIndex: this.datePeriodBackwardsIndex,
      datePeriodType: this.datePeriodType,
      dateRangeFrom: this.dateRangeFrom,
      dateRangeTo: this.dateRangeTo,
      dateRangeType: this.dateRangeType,
      groupingPeriod: this.groupingPeriod
    });
  }

  public hasHistory(): boolean {
    return this.dateAndGroupingHistory.length > 0;
  }

  public undoHistory() {
    if (this.dateAndGroupingHistory.length == 0)
      return;

    let history = this.dateAndGroupingHistory.pop();

    this.dateLastDaysRange = history.dateLastDaysRange;
    this.datePeriodBackwardsIndex = history.datePeriodBackwardsIndex;
    this.datePeriodType = history.datePeriodType;
    this.dateRangeFrom = history.dateRangeFrom;
    this.dateRangeTo = history.dateRangeTo;
    this.dateRangeType = history.dateRangeType;
    this.groupingPeriod = history.groupingPeriod;

    this.emitFilterChanged([FilterType.DATE_RANGE, FilterType.GROUPING_PERIOD]);
  }

  applyEventGroups(notSelected: number[]) {
    this.eventGroupsNotSelected = notSelected;
    this.eventGroupsChanged.next(notSelected);
  }

  applyEventSearchTerm(searchTerm: string) {
    this.eventSearchTerm = searchTerm;
    this.eventSearchTermChanged.next(searchTerm);
  }

  applyDateLastRange(dateLastDaysRange: number) {
    this.saveHistory();
    this.dateRangeType = DateRangeType.LAST;
    this.dateLastDaysRange = dateLastDaysRange;
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyDatePeriodRange(datePeriodType: PeriodType, datePeriodBackwardsIndex: number) {
    this.saveHistory();
    this.dateRangeType = DateRangeType.PERIOD;
    this.datePeriodType = datePeriodType;
    this.datePeriodBackwardsIndex = datePeriodBackwardsIndex;
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyDateRange(dateFrom: Date, dateTo: Date) {
    this.saveHistory();
    this.dateRangeType = DateRangeType.RANGE;
    this.dateRangeFrom = dateFrom;
    this.dateRangeTo = dateTo;
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyGroupingPeriod(groupingPeriod: string) {
    this.saveHistory();
    this.groupingPeriod = groupingPeriod;
    this.emitFilterChanged([FilterType.GROUPING_PERIOD]);
  }

  applyGroupingAndDatePeriodRange(groupingPeriod: string, datePeriodType: PeriodType, datePeriodBackwardsIndex: number) {
    this.saveHistory();
    this.groupingPeriod = groupingPeriod;
    this.dateRangeType = DateRangeType.PERIOD;
    this.datePeriodType = datePeriodType;
    this.datePeriodBackwardsIndex = datePeriodBackwardsIndex;
    this.emitFilterChanged([FilterType.DATE_RANGE, FilterType.GROUPING_PERIOD]);
  }

  getFromToDates() {
    return this.getFromToDatesFor(this.dateRangeType, this.dateRangeFrom, this.dateRangeTo, this.dateLastDaysRange, this.datePeriodType, this.datePeriodBackwardsIndex);
  }

  getFromToDatesFor(dateRangeType: DateRangeType, dateRangeFrom: Date, dateRangeTo: Date, dateLastDaysRange: number, datePeriodType: PeriodType, datePeriodBackwardsIndex: number): { from: Date, to: Date } {
    if (dateRangeType == DateRangeType.RANGE) {
      return {
        from: dateRangeFrom,
        to: dateRangeTo
      };
    }
    else if (dateRangeType == DateRangeType.LAST) {
      var today = new Date();
      today.setDate(today.getDate() - dateLastDaysRange + 1);
      return {
        from: today,
        to: new Date()
      };
    }
    else if (dateRangeType == DateRangeType.PERIOD) {
      var from = new Date();
      var to = new Date();

      if (datePeriodType == PeriodType.WEEK) {
        var dayOfWeekFromMonday = (from.getDay() + 7 - 1) % 7;
        from.setDate(from.getDate() - dayOfWeekFromMonday);
        to = new Date(from);
        to.setDate(from.getDate() + 6);
      }
      else if (datePeriodType == PeriodType.MONTH) {
        from.setDate(1);
        to = new Date(from);
        to.setMonth(from.getMonth() + 1);
        to.setDate(0);
      }
      else if (datePeriodType == PeriodType.YEAR) {
        from.setMonth(0);
        from.setDate(1);
        to.setMonth(11);
        to.setDate(31);
      }

      if (datePeriodType == PeriodType.DAY) {
        from.setDate(from.getDate() - datePeriodBackwardsIndex);
        to.setDate(to.getDate() - datePeriodBackwardsIndex);
      }
      else if (datePeriodType == PeriodType.WEEK) {
        from.setDate(from.getDate() - datePeriodBackwardsIndex * 7);
        to.setDate(to.getDate() - datePeriodBackwardsIndex * 7);
      }
      else if (datePeriodType == PeriodType.MONTH) {
        from.setMonth(from.getMonth() - datePeriodBackwardsIndex);
        to = new Date(from);
        to.setMonth(from.getMonth() + 1);
        to.setDate(0);
      }
      else if (datePeriodType == PeriodType.YEAR) {
        from.setFullYear(from.getFullYear() - datePeriodBackwardsIndex);
        to.setFullYear(to.getFullYear() - datePeriodBackwardsIndex);
      }

      return {
        from: from,
        to: to
      };
    }

    return {
      from: new Date(),
      to: new Date()
    };
  }

  static getBackwardsIndexForDate(date: Date, periodType: PeriodType): number {
    if (periodType == PeriodType.DAY) {
      let today = new Date();
      today.setHours(0, 0, 0, 0);
      return Math.round((<any>today - <any>date) / (1000 * 60 * 60 * 24));
    }
    else if (periodType == PeriodType.WEEK) {
      let firstDayOfWeek = new Date();
      firstDayOfWeek.setHours(0, 0, 0, 0);
      var dayOfWeekFromMonday = (firstDayOfWeek.getDay() + 7 - 1) % 7;
      firstDayOfWeek.setDate(firstDayOfWeek.getDate() - dayOfWeekFromMonday);

      return Math.round((<any>firstDayOfWeek - <any>date) / (7 * 24 * 60 * 60 * 1000));
    }
    else if (periodType == PeriodType.MONTH) {
      let firstDayOfMonth = new Date();
      firstDayOfMonth.setHours(0, 0, 0, 0);
      firstDayOfMonth.setDate(1);
      return firstDayOfMonth.getMonth() - date.getMonth() + (12 * (firstDayOfMonth.getFullYear() - date.getFullYear()))
    }
    else if (periodType == PeriodType.YEAR) {
      let firstDayOfYear = new Date();
      firstDayOfYear.setHours(0, 0, 0, 0);
      firstDayOfYear.setMonth(0, 1)
      return firstDayOfYear.getFullYear() - date.getFullYear();
    }
    else
      throw "Invalid periodType";
  }

}

export enum DateRangeType {
  LAST,
  PERIOD,
  RANGE
}

export enum PeriodType {
  DAY,
  WEEK,
  MONTH,
  YEAR
}

export enum FilterType {
  DATE_RANGE,
  GROUPING_PERIOD
}