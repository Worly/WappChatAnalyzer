import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { appConfig } from "src/app/app.config";

@Injectable({ providedIn: "root" })
export class AuthService {
    private readonly TOKEN_KEY = "JWT_TOKEN";

    private token: string;

    constructor(private router: Router, private httpClient: HttpClient) {
        this.token = localStorage.getItem(this.TOKEN_KEY);
    }

    public logIn(email: string, password: string): Observable<any> {
        return this.httpClient.post(appConfig.apiUrl + "user/login", {
            email: email,
            password: password,
        }).pipe(
            tap(o => this.setToken(o.token))
        );
    }

    public register(email: string, username: string, password: string): Observable<any> {
        return this.httpClient.post(appConfig.apiUrl + "user/register", {
            email: email,
            username: username,
            password: password
        }).pipe(
            tap(o => this.setToken(o.token))
        );
    }

    private setToken(token: string): void {
        this.token = token;
        localStorage.setItem(this.TOKEN_KEY, token);
    }

    public getToken(): string {
        return this.token;
    }

    public isLoggedIn(): boolean {
        return this.token != null;
    }

    public logOut(): void {
        this.token = null;
        localStorage.removeItem(this.TOKEN_KEY);
        this.router.navigate(["login"]);
    }
}