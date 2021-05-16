import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { appConfig } from '../app.config';
import { ImportHistory } from "../dtos/importHistory";
import { Message } from "../dtos/message";
import * as dateFormat from "dateformat";

@Injectable({
    providedIn: 'root'
})
export class MessagesService {

    constructor(private http: HttpClient) { }

    public uploadChatExport(file: File) {
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);
        return this.http.post(appConfig.apiUrl + "messages/uploadChatExport", formData);
    }

    public getImportHistory(): Observable<ImportHistory[]> {
        return <Observable<ImportHistory[]>>this.http.get(appConfig.apiUrl + "messages/getImportHistory");
    }

    public getLastMessageId(): Observable<number> {
        return <Observable<number>>this.http.get(appConfig.apiUrl + "messages/getLastMessageId");
    }

    public getMessages(fromId: number, toId: number): Observable<Message[]> {
        return <Observable<Message[]>>this.http.get(appConfig.apiUrl + "messages/getMessages", {
            params: <any>{
                "fromId": fromId,
                "toId": toId
            }
        });
    }

    public getFirstMessageOfDayBefore(date: Date): Observable<Message> {
        return <Observable<Message>>this.http.get(appConfig.apiUrl + "messages/getFirstMessageOfDayBefore", {
            params: <any>{
                "dateTime": dateFormat(date, "yyyy-mm-dd")
            }
        });
    }

    public getFirstMessageOfDayAfter(date: Date): Observable<Message> {
        return <Observable<Message>>this.http.get(appConfig.apiUrl + "messages/getFirstMessageOfDayAfter", {
            params: <any>{
                "dateTime": dateFormat(date, "yyyy-mm-dd")
            }
        });
    }

    public getAllSenders(): Observable<string[]> {
        return <Observable<string[]>>this.http.get(appConfig.apiUrl + "messages/getAllSenders");
    }
}