import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { appConfig } from "src/app/app.config";
import jwt_decode from "jwt-decode";
import { WorkspaceService } from "../workspaces/workspace.service";

@Injectable({ providedIn: "root" })
export class AuthService {
    private readonly TOKEN_KEY = "JWT_TOKEN";

    private token: string;

    constructor(private router: Router, private httpClient: HttpClient, private workspaceService: WorkspaceService) {
    }

    public loadFromLocalStorage(): Observable<void> {
        return new Observable<void>(s => {
            var obs = this.setToken(localStorage.getItem(this.TOKEN_KEY));
            if (!obs)
                s.next();

            obs.subscribe({
                next: () => s.next(),
                error: () => s.error()
            });
        });
    }

    public logIn(email: string, password: string): Observable<void> {
        return new Observable<void>(s => {
            this.httpClient.post<any>(appConfig.apiUrl + "user/login", {
                email: email,
                password: password,
            }).subscribe({
                next: o => {
                    this.setToken(o.token).subscribe({
                        next: () => s.next(),
                        error: o => {
                            this.logOut(false);
                            s.error(o)
                        }
                    });
                },
                error: o => s.error(o)
            });
        });
    }

    public register(email: string, username: string, password: string): Observable<void> {
        return new Observable<void>(s => {
            this.httpClient.post<any>(appConfig.apiUrl + "user/register", {
                email: email,
                username: username,
                password: password
            }).subscribe({
                next: o => {
                    this.setToken(o.token).subscribe({
                        next: () => s.next(),
                        error: o => {
                            this.logOut(false);
                            s.error(o)
                        }
                    });
                },
                error: o => s.error(o)
            });
        });
    }

    public verifyEmail(token: string): Observable<void> {
        return this.httpClient.post<void>(appConfig.apiUrl + "user/verifyEmail", null, {
            params: { token: token }
        });
    }

    public requestVerificationEmail(): Observable<void> {
        return this.httpClient.post<void>(appConfig.apiUrl + "user/requestVerificationEmail", null);
    }

    private setToken(token: string): Observable<void> {
        if (token == null) {
            this.logOut(false);
            return null;
        }

        this.token = token;
        localStorage.setItem(this.TOKEN_KEY, token);

        return <any>this.workspaceService.loadMy();
    }

    public getToken(): string {
        return this.token;
    }

    public isLoggedIn(): boolean {
        return this.token != null;
    }

    public logOut(navigateToLogin: boolean = true): void {
        this.token = null;
        localStorage.removeItem(this.TOKEN_KEY);
        this.workspaceService.clear();

        if (navigateToLogin)
            this.router.navigate(["login"]);
    }

    public getUsername(): string {
        if (!this.isLoggedIn())
            return null;

        var decoded = <any>jwt_decode(this.token);
        return decoded.username;
    }

    public getVerifiedEmail(): boolean {
        if (!this.isLoggedIn())
            return null;

        var decoded = <any>jwt_decode(this.token);
        return decoded.verifiedEmail;
    }
}