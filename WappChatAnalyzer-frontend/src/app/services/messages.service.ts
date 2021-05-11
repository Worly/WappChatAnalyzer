import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { appConfig } from '../app.config';
import { ImportHistory } from "../dtos/importHistory";

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
}