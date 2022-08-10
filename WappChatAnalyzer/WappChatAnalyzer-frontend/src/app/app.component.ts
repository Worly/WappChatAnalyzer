import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from './services/auth/auth.service';
import { WorkspaceService } from './services/workspaces/workspace.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  @ViewChild("navbar") navbar: ElementRef<HTMLElement>;

  constructor(
    private router: Router,
    public authService: AuthService, 
    public workspaceService: WorkspaceService) {}

  ngOnInit() {
    this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd)
        this.closeNavbar();
    })
  }

  public logOut(): void {
    this.authService.logOut();
  }
  
  closeNavbar() {
    this.navbar.nativeElement.classList.remove("show");
  }
}
