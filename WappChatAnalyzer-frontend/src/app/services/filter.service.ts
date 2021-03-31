import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  eventGroupsNotSelected: number[] = [];
  eventGroupsChanged: Subject<number[]> = new Subject<number[]>();


  dateRangeType: DateRangeType = DateRangeType.LAST;

  dateLastDaysRange: number = 30;

  datePeriodType: PeriodType = PeriodType.MONTH;
  datePeriodBackwardsIndex: number = 0;

  dateRangeFrom: Date;
  dateRangeTo: Date;

  dateFilterChanged: Subject<{ from: Date, to: Date }> = new Subject<{ from: Date, to: Date }>();

  constructor() {
    this.dateRangeTo = new Date();

    var today = new Date();
    today.setDate(today.getDate() - 30);
    this.dateRangeFrom = today;
  }

  applyEventGroups(notSelected: number[]) {
    this.eventGroupsNotSelected = notSelected;
    this.eventGroupsChanged.next(notSelected);
  }

  applyDateLastRange(dateLastDaysRange: number) {
    this.dateRangeType = DateRangeType.LAST;
    this.dateLastDaysRange = dateLastDaysRange;
    this.dateFilterChanged.next(this.getFromToDates());
  }

  applyDatePeriodRange(datePeriodType: PeriodType, datePeriodBackwardsIndex: number) {
    this.dateRangeType = DateRangeType.PERIOD;
    this.datePeriodType = datePeriodType;
    this.datePeriodBackwardsIndex = datePeriodBackwardsIndex;
    this.dateFilterChanged.next(this.getFromToDates());
  }

  applyDateRange(dateFrom: Date, dateTo: Date) {
    this.dateRangeType = DateRangeType.RANGE;
    this.dateRangeFrom = dateFrom;
    this.dateRangeTo = dateTo;
    this.dateFilterChanged.next(this.getFromToDates());
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