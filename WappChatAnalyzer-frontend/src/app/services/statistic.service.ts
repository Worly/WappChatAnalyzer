import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { StatisticTotal } from '../dtos/statisticTotal';
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
    params["per"] = this.filterService.per;
    params["perReciprocal"] = this.filterService.perReciprocal;

    return params;
  }

  getStatisticTotal(apiUrl: string): Observable<StatisticTotal> {
    return <Observable<StatisticTotal>>this.http.get(appConfig.apiUrl + apiUrl, {
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

  saveCustomStatistic(customStatistic: CustomStatistic): Observable<CustomStatistic> {
    return <Observable<CustomStatistic>>this.http.post(appConfig.apiUrl + "statistic/saveCustomStatistic", customStatistic);
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
