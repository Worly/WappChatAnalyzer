import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {

  }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      catchError((e: HttpEvent<any>) => {
        if (e instanceof HttpErrorResponse) {
          if (e.status == 400) // ignore bad requests
            return throwError(e);

          if (e.status == 401) // ignore unauthorized
            return throwError(e);

          if (e.status == 403 && e.error.message == "Email not verified") // ignore emailNotVerified
            return throwError(e);

          this.router.navigate(["error"], {
            state: {
              error: {
                status: e.status,
                statusText: e.statusText,
                message: e.message
              }
            }
          });
        }
        return throwError(e);
      })
    );
  }
}