import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Subject, Unsubscribable } from 'rxjs';
import * as _ from "lodash";
import * as dateFormat from "dateformat";

@Injectable({
  providedIn: 'root'
})
export class FilterService {

  eventFilters: EventFilters = new EventFilters();

  eventGroupsChanged: Subject<number[]> = new Subject<number[]>();
  eventSearchTermChanged: Subject<string> = new Subject<string>();


  statisticFilters: StatisticFilters = new StatisticFilters();

  private statisticFiltersHistory: StatisticFilters[] = [];

  private filterChangedSubscribers: { subscribedTo: FilterType[], callback: () => void }[] = [];

  constructor(private location: Location, private router: Router, private activatedRoute: ActivatedRoute) {
    this.getFiltersFromURLParams();

    router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        this.applyFiltersToURLParams();
      }
    });
  }

  private dateOnlyJSONReplacer(key, value) {
    if (value instanceof Date || (typeof value === "string" && !isNaN(Date.parse(value))))
      return dateFormat(value, "yyyy-mm-dd");
    else
      return value;
  }

  private getOnlyDifferences<T>(old: T, newObj: T): object {
    let result = {};

    for (let key in old) {
      if (!_.isEqualWith(old[key], newObj[key], (f, s) => {
        if (f instanceof Date)
          return dateFormat(f, "yyyy-mm-dd") == dateFormat(new Date(s), "yyyy-mm-dd");
        return _.isEqual(f, s);
      }))
        result[key.toString()] = newObj[key];
    }

    return result;
  }

  private URLParamsToObject(): any {
    let result = {};
    for (let key of this.activatedRoute.snapshot.queryParamMap.keys)
      result[key] = this.activatedRoute.snapshot.queryParamMap.get(key);
    return result;
  }

  private applyFiltersToURLParams() {
    let differencesForEventFilters = this.getOnlyDifferences(EventFilters.DEFAULT, this.eventFilters);
    let differencesForStatisticFilters = this.getOnlyDifferences(StatisticFilters.DEFAULT, this.statisticFilters);

    let url = this.router.createUrlTree([], {
      relativeTo: this.activatedRoute,
      queryParams: {
        ...this.URLParamsToObject(),
        eventFilters: _.isEqual(differencesForEventFilters, {}) ? undefined : JSON.stringify(differencesForEventFilters, this.dateOnlyJSONReplacer),
        statisticFilters: _.isEqual(differencesForStatisticFilters, {}) ? undefined : JSON.stringify(differencesForStatisticFilters, this.dateOnlyJSONReplacer)
      }
    }).toString();
    this.location.replaceState(url);
  }

  private getFiltersFromURLParams() {
    let params = this.URLParamsToObject();

    if (params == null)
      return;

    if (params.eventFilters != null)
      this.eventFilters = {
        ...this.eventFilters,
        ...JSON.parse(params.eventFilters)
      };

    if (params.statisticFilters != null)
      this.statisticFilters = {
        ...this.statisticFilters,
        ...JSON.parse(params.statisticFilters)
      };
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
    this.statisticFiltersHistory.push({...this.statisticFilters});
  }

  public hasHistory(): boolean {
    return this.statisticFiltersHistory.length > 0;
  }

  public undoHistory() {
    if (this.statisticFiltersHistory.length == 0)
      return;

    this.statisticFilters = this.statisticFiltersHistory.pop();
    
    this.applyFiltersToURLParams();

    this.emitFilterChanged([FilterType.DATE_RANGE, FilterType.GROUPING_PERIOD, FilterType.PER]);
  }

  applyEventGroups(notSelected: number[]) {
    this.eventFilters.eventGroupsNotSelected = notSelected;
    this.applyFiltersToURLParams();
    this.eventGroupsChanged.next(notSelected);
  }

  applyEventSearchTerm(searchTerm: string) {
    this.eventFilters.eventSearchTerm = searchTerm;
    this.applyFiltersToURLParams();
    this.eventSearchTermChanged.next(searchTerm);
  }

  applyDateLastRange(dateLastDaysRange: number) {
    this.saveHistory();
    this.statisticFilters.dateRangeType = DateRangeType.LAST;
    this.statisticFilters.dateLastDaysRange = dateLastDaysRange;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyDatePeriodRange(datePeriodType: PeriodType, datePeriodBackwardsIndex: number) {
    this.saveHistory();
    this.statisticFilters.dateRangeType = DateRangeType.PERIOD;
    this.statisticFilters.datePeriodType = datePeriodType;
    this.statisticFilters.datePeriodBackwardsIndex = datePeriodBackwardsIndex;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyDateRange(dateFrom: Date, dateTo: Date) {
    this.saveHistory();
    this.statisticFilters.dateRangeType = DateRangeType.RANGE;
    this.statisticFilters.dateRangeFrom = dateFrom;
    this.statisticFilters.dateRangeTo = dateTo;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.DATE_RANGE]);
  }

  applyGroupingPeriod(groupingPeriod: string) {
    this.saveHistory();
    this.statisticFilters.groupingPeriod = groupingPeriod;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.GROUPING_PERIOD]);
  }

  applyPer(per: string, perReciprocal: boolean) {
    this.saveHistory();
    this.statisticFilters.per = per;
    this.statisticFilters.perReciprocal = perReciprocal;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.PER]);
  }

  applyGroupingAndDatePeriodRange(groupingPeriod: string, datePeriodType: PeriodType, datePeriodBackwardsIndex: number) {
    this.saveHistory();
    this.statisticFilters.groupingPeriod = groupingPeriod;
    this.statisticFilters.dateRangeType = DateRangeType.PERIOD;
    this.statisticFilters.datePeriodType = datePeriodType;
    this.statisticFilters.datePeriodBackwardsIndex = datePeriodBackwardsIndex;
    this.applyFiltersToURLParams();
    this.emitFilterChanged([FilterType.DATE_RANGE, FilterType.GROUPING_PERIOD]);
  }

  getFromToDates() {
    return this.getFromToDatesFor(
      this.statisticFilters.dateRangeType,
      this.statisticFilters.dateRangeFrom,
      this.statisticFilters.dateRangeTo,
      this.statisticFilters.dateLastDaysRange,
      this.statisticFilters.datePeriodType,
      this.statisticFilters.datePeriodBackwardsIndex);
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
        from: dateLastDaysRange == -1 ? null : today,
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
  GROUPING_PERIOD,
  PER
}

export class EventFilters {
  public eventGroupsNotSelected: number[] = [];
  public eventSearchTerm: string = "";

  public static DEFAULT: EventFilters = new EventFilters();
}

export class StatisticFilters {
  public dateRangeType: DateRangeType = DateRangeType.LAST;

  public dateLastDaysRange: number = 30;

  public datePeriodType: PeriodType = PeriodType.MONTH;
  public datePeriodBackwardsIndex: number = 0;

  public dateRangeFrom: Date;
  public dateRangeTo: Date = new Date();

  public groupingPeriod: string = "date";

  public per: string = "none";
  public perReciprocal: boolean = false;

  public static DEFAULT: StatisticFilters = new StatisticFilters();
  public constructor() {
    if (this.dateRangeFrom == null) {
      var today = new Date();
      today.setDate(today.getDate() - 30);
      this.dateRangeFrom = today;
    }
  }
}