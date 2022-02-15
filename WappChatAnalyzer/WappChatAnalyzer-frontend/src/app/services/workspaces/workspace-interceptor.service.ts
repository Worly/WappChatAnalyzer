import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkspaceService } from './workspace.service';

@Injectable()
export class WorkspaceInterceptor implements HttpInterceptor {

  constructor(private workspaceService: WorkspaceService) {

  }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.workspaceService.getSelected()) {
      httpRequest = httpRequest.clone({
        setHeaders: { "workspace-id": this.workspaceService.getSelected().id.toString() }
      });
    }

    return next.handle(httpRequest);
  }
}