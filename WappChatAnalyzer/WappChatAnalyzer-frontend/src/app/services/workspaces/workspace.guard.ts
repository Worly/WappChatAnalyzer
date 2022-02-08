import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { WorkspaceService } from "./workspace.service";

@Injectable({ providedIn: "root" })
export class SelectedWorkspaceGuard implements CanActivate {
    constructor(private workspaceService: WorkspaceService, private router: Router) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.workspaceService.getSelected() == null)
            return this.router.createUrlTree(["/workspaces"]);

        return true;
    }
}