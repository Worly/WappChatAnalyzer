import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { Emoji } from '../dtos/emoji';
import { Statistic } from '../dtos/statistic';

@Injectable({
  providedIn: 'root'
})
export class EmojiService {

    getEmojiIcon(name: string, codePoints: string) {

        return
    }
}