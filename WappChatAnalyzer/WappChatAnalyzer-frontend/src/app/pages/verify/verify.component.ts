import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { appConfig } from 'src/app/app.config';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-verify',
  templateUrl: './verify.component.html',
  styleUrls: ['./verify.component.css']
})
export class VerifyComponent implements OnInit {
  status: "loading" | "success" | "error" = "loading";

  constructor(
    public activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.queryParamMap.subscribe(q => {
      if (!q.has("token")) {
        this.status = "error";
        return;
      }

      let token = q.get("token");
      this.status = "loading";
      this.authService.verifyEmail(token).subscribe({
        next: () => this.status = "success",
        error: () => this.status = "error"
      });
    });
  }
}