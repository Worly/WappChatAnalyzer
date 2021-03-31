import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { Emoji } from '../dtos/emoji';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { Statistic } from '../dtos/statistic';
import { FilterService } from './filter.service';
import * as dateFormat from "dateformat";

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient, private filterService: FilterService) { }

  private getParams() {
    var params = {};

    var format = "yyyy-mm-dd";
    var fromToDates = this.filterService.getFromToDates();
    if (fromToDates.from != null)
      params["fromDate"] = dateFormat(fromToDates.from, format);
    if (fromToDates.to != null)
      params["toDate"] = dateFormat(fromToDates.to, format);

    return params;
  }

  getBasicInfo(): Observable<BasicInfoTotal> {
    return <Observable<BasicInfoTotal>>this.http.get(appConfig.apiUrl + "basic/getBasicInfoTotal", {
      params: this.getParams()
    });
  }

  getStatistic(statisticUrl: string): Observable<Statistic> {
    return <Observable<Statistic>>this.http.get(appConfig.apiUrl + statisticUrl, {
      params: this.getParams()
    });
  }

  getEmojiInfoTotal(): Observable<EmojiInfoTotal> {
    return <Observable<EmojiInfoTotal>>this.http.get(appConfig.apiUrl + "emoji/getEmojiInfoTotal", {
      params: this.getParams()
    });
  }

  getEmojiByCodePoints(codePoints: string): Observable<Emoji> {
    return <Observable<Emoji>>this.http.get(appConfig.apiUrl + "emoji/getEmoji", {
      params: {
        emojiCodePoints: codePoints
      }
    });
  }
}
