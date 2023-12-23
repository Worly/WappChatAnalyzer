import { ViewportRuler } from '@angular/cdk/scrolling';
import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { WorkspaceService } from 'src/app/services/workspaces/workspace.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public height: number;

  public validationErrors: {
    [key: string]: string
  } = {};

  public email: string;
  public password: string;

  public isLoading: boolean = false;

  private readonly viewportChange = this.viewportRuler
    .change(50)
    .subscribe(() => this.ngZone.run(() => this.setHeight()));

  constructor(
    private authService: AuthService,
    private workspaceService: WorkspaceService,
    private router: Router,
    private readonly viewportRuler: ViewportRuler,
    private readonly ngZone: NgZone
  ) {
  }

  ngOnInit(): void {
    this.setHeight();
  }

  setHeight(): void {
    this.height = this.viewportRuler.getViewportSize().height - 300;
  }

  keypress(e: KeyboardEvent): void {
    if (e.key == "Enter")
      this.login();
  }

  validate(): boolean {
    this.validationErrors = {};

    if (this.email == null || this.email == "")
      this.validationErrors["email"] = "Please enter your email";

    if (this.password == null || this.password == "")
      this.validationErrors["password"] = "Please enter your password";

    return Object.entries(this.validationErrors).length == 0;
  }

  login(): void {
    if (this.validate()) {
      this.isLoading = true;
      this.authService.logIn(this.email, this.password).subscribe({
        next: o => {
          this.isLoading = false;
          
          if (this.workspaceService.getSelected() == null)
            this.router.navigate(["workspaces"]);
          else
            this.router.navigate(["events"]);
        },
        error: e => {
          this.isLoading = false;
          this.validationErrors = e.error.errors;
        },
      });
    }
  }
}
