import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ToastComponent } from '../toast/toast.component';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.css']
})
export class VerifyEmailComponent implements OnInit, OnDestroy {

  @Input() action: string;

  private readonly TIMEOUT = 10;

  isLoading: boolean = false;

  lastTime: number | null = null;
  intervalHandle: any;

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.intervalHandle = setInterval(() => {
      this.render();
    }, 200);
  }

  ngOnDestroy(): void {
    clearInterval(this.intervalHandle);
  }

  requestVerifyEmail() {
    this.lastTime = Date.now();
    this.isLoading = true;
    this.authService.requestVerificationEmail().subscribe(() => {
      this.isLoading = false;
    });
  }

  render() {

  }

  getText(): string {
    var text = "Request a new verification email";
    
    if (this.isDisabled())
      text += " (" + (this.TIMEOUT - Math.floor((Date.now() - this.lastTime) / 1000)) + ")";

    return text;
  }

  isDisabled(): boolean {
    return this.isLoading || (this.lastTime != null && Math.floor((Date.now() - this.lastTime) / 1000) < this.TIMEOUT);
  }

}
