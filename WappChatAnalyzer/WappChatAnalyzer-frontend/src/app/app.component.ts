import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import * as CanvasJS from '../assets/canvasjs.min';
import { Statistic } from "./dtos/statistic";
import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(public authService: AuthService) {}

  ngOnInit() {
    
  }

  public logOut(): void {
    this.authService.logOut();
  }
  
}
