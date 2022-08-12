import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { forkJoin, Observable, of, throwError } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { WorkspaceShare } from "src/app/dtos/workspaceShare";
import { appConfig } from "../../app.config";
import { Workspace } from "../../dtos/workspace";

@Injectable({ providedIn: "root" })
export class WorkspaceService {

    public myWorkspaces: Workspace[];
    public sharedWorkspaces: Workspace[] | null;
    private selectedWorkspaceId: number;

    constructor(private http: HttpClient) { }

    public loadMy(): Observable<Workspace[]> {
        var getMy = this.http.get<Workspace[]>(appConfig.apiUrl + "workspace/getMy");
        var getShared = this.http.get<Workspace[] | null>(appConfig.apiUrl + "workspace/getShared").pipe(catchError((e: HttpErrorResponse) => {
            if (e.status == 403 && e.error.message == "Email not verified")
                return of(null);
            else
                return throwError(e);
        }));
        var getSelected = this.http.get<number>(appConfig.apiUrl + "workspace/getSelectedWorkspace");

        return forkJoin([getMy, getShared, getSelected])
            .pipe(map(([myWorkspaces, sharedWorkspaces, selectedId]) => {
                this.myWorkspaces = myWorkspaces;
                this.sharedWorkspaces = sharedWorkspaces;
                this.selectedWorkspaceId = selectedId;
                return myWorkspaces;
            }));
    }

    public reloadShared() {
        this.http.get<Workspace[]>(appConfig.apiUrl + "workspace/getShared").subscribe(w => {
            this.sharedWorkspaces = w;
        });
    }

    public getSelected(): Workspace {
        if (this.selectedWorkspaceId == null || this.myWorkspaces == null)
            return null;

        let workspace = this.myWorkspaces.find(o => o.id == this.selectedWorkspaceId);
        if (workspace == null && this.sharedWorkspaces != null)
            workspace = this.sharedWorkspaces.find(o => o.id == this.selectedWorkspaceId)
            
        return workspace;
    }

    public selectWorkspace(workspace: Workspace): Observable<void> {
        return this.http.put<void>(appConfig.apiUrl + "workspace/selectWorkspace", workspace.id)
            .pipe(tap(() => this.selectedWorkspaceId = workspace.id));
    }

    public deleteWorkspace(workspace: Workspace): Observable<void> {
        return this.http.delete<void>(appConfig.apiUrl + "workspace/delete/" + workspace.id)
            .pipe(tap(() => {
                var index = this.myWorkspaces.indexOf(workspace);
                if (index > -1)
                    this.myWorkspaces.splice(index, 1);

                if (this.selectedWorkspaceId == workspace.id)
                    this.selectedWorkspaceId = null;
            }));
    }

    public addNew(name: string): Observable<Workspace> {
        return this.http.post<Workspace>(appConfig.apiUrl + "workspace/addNew", {
            name: name
        }).pipe(tap(w => {
            this.myWorkspaces.push(w);
        }));
    }

    public edit(workspaceId: number, newName: string): Observable<Workspace> {
        return this.http.put<Workspace>(appConfig.apiUrl + "workspace/edit/" + workspaceId, {
            name: newName
        }).pipe(tap(w => {
            var oldWorkspace = this.myWorkspaces.find(o => o.id == w.id);
            oldWorkspace.name = w.name;
        }));
    }

    public getWorkspaceShares(workspaceId: number): Observable<WorkspaceShare[]> {
        return this.http.get<WorkspaceShare[]>(appConfig.apiUrl + "workspace/getWorkspaceShares/" + workspaceId);
    }

    public shareWorkspace(workspaceId: number, usersEmail: string): Observable<WorkspaceShare[]> {
        return this.http.post<WorkspaceShare[]>(appConfig.apiUrl + "workspace/shareWorkspace/" + workspaceId, {
            sharedUserEmail: usersEmail
        });
    }

    public unshareWorkspace(workspaceId: number, usersEmail: string): Observable<WorkspaceShare[]> {
        return this.http.post<WorkspaceShare[]>(appConfig.apiUrl + "workspace/unshareWorkspace/" + workspaceId, {
            sharedUserEmail: usersEmail
        });
    }

    public clear() {
        this.myWorkspaces = null;
    }
}