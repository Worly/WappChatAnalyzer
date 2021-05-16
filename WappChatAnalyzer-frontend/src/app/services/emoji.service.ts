import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { appConfig } from '../app.config';
import { Emoji } from '../dtos/emoji';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class EmojiService {

    private emojiCache: { [Key: string]: Emoji } = {};
    private emojiObservableCache: { [Key: string]: Subject<Emoji> } = {};

    constructor(private http: HttpClient) { }

    getEmojiByCodePoints(codePoints: string): Observable<Emoji> {
        if (this.emojiCache.hasOwnProperty(codePoints))
            return of(this.emojiCache[codePoints]);
        else if (this.emojiObservableCache.hasOwnProperty(codePoints)) {
            return this.emojiObservableCache[codePoints];
        }
        else {
            this.emojiObservableCache[codePoints] = new Subject<Emoji>();
            this.http.get(appConfig.apiUrl + "emoji/getEmoji", {
                params: {
                    emojiCodePoints: codePoints
                }
            }).subscribe((r: Emoji) => {
                this.emojiCache[codePoints] = r
                this.emojiObservableCache[codePoints].next(r);
                delete this.emojiObservableCache[codePoints];
            });
            return this.emojiObservableCache[codePoints];
        }
    }

    getLinkToWappEmojiImage(emoji: Emoji): string {
        let name = emoji.name.toLowerCase().split(' ').join("-").replace(":", "");
        let codePoints = emoji.codePoints.toLowerCase().split(' ').join('-');

        return "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/72/whatsapp/273/" + name + "_" + codePoints + ".png";
    }
}
