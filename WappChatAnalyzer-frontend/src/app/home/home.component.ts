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

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.dataService.getBasicInfo().subscribe(r => {
      this.basicInfoTotal = r;
    });
  }

}
