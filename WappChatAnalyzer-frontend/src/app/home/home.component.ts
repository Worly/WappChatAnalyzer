import { Component, OnInit } from '@angular/core';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public basicInfoTotal: BasicInfoTotal;

  public isLoading: boolean;

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.dataService.getBasicInfo().subscribe(r => {
      this.isLoading = false;
      this.basicInfoTotal = r;
    });
  }

}
