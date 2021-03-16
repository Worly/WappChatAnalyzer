import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { Emoji } from '../dtos/emoji';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { Statistic } from '../dtos/statistic';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private apiUrl = "https://localhost:5001/";

  constructor(private http: HttpClient) { }

  getBasicInfo(): Observable<BasicInfoTotal> {
    return <Observable<BasicInfoTotal>>this.http.get(this.apiUrl + "basic/getBasicInfoTotal");
  }

  getStatistic(statisticName: string): Observable<Statistic> {
    return <Observable<Statistic>>this.http.get(this.apiUrl + "basic/getStatistic?statisticName=" + statisticName);
  }

  getEmojiInfoTotal(): Observable<EmojiInfoTotal> {
    return <Observable<EmojiInfoTotal>>this.http.get(this.apiUrl + "emoji/getEmojiInfoTotal");
  }
  
  getEmojiByCodePoints(codePoints: string): Observable<Emoji> {
    return <Observable<Emoji>>this.http.get(this.apiUrl + "emoji/getEmoji", { 
      params: {
        emojiCodePoints: codePoints
      }
    });
  }
}
