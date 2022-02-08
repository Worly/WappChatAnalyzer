import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { forkJoin, Observable } from "rxjs";
import { map, tap } from "rxjs/operators";
import { appConfig } from "../../app.config";
import { Workspace } from "../../dtos/workspace";

@Injectable({ providedIn: "root" })
export class WorkspaceService {

    public myWorkspaces: Workspace[];
    private selectedWorkspaceId: number;

    constructor(private http: HttpClient) { }

    public loadMy(): Observable<Workspace[]> {
        var getMy = this.http.get<Workspace[]>(appConfig.apiUrl + "workspace/getMy");
        var getSelected = this.http.get<number>(appConfig.apiUrl + "workspace/getSelectedWorkspace");

        return forkJoin([getMy, getSelected])
            .pipe(map(([workspaces, selectedId]) => {
                this.myWorkspaces = workspaces;
                this.selectedWorkspaceId = selectedId;
                return workspaces;
            }));
    }

    public getSelected(): Workspace {
        if (this.selectedWorkspaceId == null || this.myWorkspaces == null)
            return null;

        return this.myWorkspaces.find(o => o.id == this.selectedWorkspaceId);
    }

    public selectWorkspace(workspace: Workspace): Observable<void> {
        return this.http.put<void>(appConfig.apiUrl + "workspace/selectWorkspace", workspace.id)
            .pipe(tap(() => this.selectedWorkspaceId = workspace.id));
    }

    public clear() {
        this.myWorkspaces = null;
    }
}