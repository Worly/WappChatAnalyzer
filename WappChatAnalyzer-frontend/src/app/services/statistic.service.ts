import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { BasicInfoTotal, StatisticTotal } from '../dtos/statisticTotal';
import { Emoji } from '../dtos/emoji';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { Statistic } from '../dtos/statistic';
import { FilterService } from './filter.service';
import * as dateFormat from "dateformat";
import { CustomStatistic } from '../dtos/customStatistic';

@Injectable({
  providedIn: 'root'
})
export class StatisticService {

  constructor(private http: HttpClient, private filterService: FilterService) { }

  private getParams() {
    var params = {};

    var format = "yyyy-mm-dd";
    var fromToDates = this.filterService.getFromToDates();
    if (fromToDates.from != null)
      params["fromDate"] = dateFormat(fromToDates.from, format);
    if (fromToDates.to != null)
      params["toDate"] = dateFormat(fromToDates.to, format);

    params["groupingPeriod"] = this.filterService.groupingPeriod;

    return params;
  }

  getBasicInfo(): Observable<BasicInfoTotal> {
    return <Observable<BasicInfoTotal>>this.http.get(appConfig.apiUrl + "statistic/getBasicInfoTotal", {
      params: this.getParams()
    });
  }

  getStatistic(statisticUrl: string): Observable<Statistic> {
    return <Observable<Statistic>>this.http.get(appConfig.apiUrl + statisticUrl, {
      params: this.getParams()
    });
  }

  getCustomStatistics(): Observable<CustomStatistic[]> {
    return <Observable<CustomStatistic[]>>this.http.get(appConfig.apiUrl + "statistic/getCustomStatistics")
  }

  getCustomStatistic(id: number): Observable<CustomStatistic> {
    return <Observable<CustomStatistic>>this.http.get(appConfig.apiUrl + "statistic/getCustomStatistic/" + id);
  }

  getCustomStatisticTotal(id: number): Observable<StatisticTotal> {
    return <Observable<StatisticTotal>>this.http.get(appConfig.apiUrl + "statistic/getCustomStatisticTotal/" + id, {
      params: this.getParams()
    });
  }

  getEmojiInfoTotal(): Observable<EmojiInfoTotal> {
    return <Observable<EmojiInfoTotal>>this.http.get(appConfig.apiUrl + "emoji/getEmojiInfoTotal", {
      params: this.getParams()
    });
  }
}
