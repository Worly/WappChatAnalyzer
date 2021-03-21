import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { appConfig } from '../app.config';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { Emoji } from '../dtos/emoji';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { Statistic } from '../dtos/statistic';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient) { }

  getBasicInfo(): Observable<BasicInfoTotal> {
    return <Observable<BasicInfoTotal>>this.http.get(appConfig.apiUrl + "basic/getBasicInfoTotal");
  }

  getStatistic(statisticUrl: string): Observable<Statistic> {
    return <Observable<Statistic>>this.http.get(appConfig.apiUrl + statisticUrl);
  }

  getEmojiInfoTotal(): Observable<EmojiInfoTotal> {
    return <Observable<EmojiInfoTotal>>this.http.get(appConfig.apiUrl + "emoji/getEmojiInfoTotal");
  }
  
  getEmojiByCodePoints(codePoints: string): Observable<Emoji> {
    return <Observable<Emoji>>this.http.get(appConfig.apiUrl + "emoji/getEmoji", { 
      params: {
        emojiCodePoints: codePoints
      }
    });
  }
}
