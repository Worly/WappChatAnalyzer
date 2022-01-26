import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpResponse, HttpRequest, HttpHandler, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {
  
  constructor(private router: Router) {

  }

  intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(httpRequest).pipe(
      catchError((e: HttpEvent<any>) => {
        if (e instanceof HttpErrorResponse) {
          if (e.status == 401) // Unauthorized
          {
            console.log("Unauthorized");
            this.router.navigate(["login"]);
          }
        }
        return of(e);
      })
    );
  }
}