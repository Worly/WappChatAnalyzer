import { Component, Input, OnInit } from '@angular/core';
import { BasicInfoTotal } from '../dtos/basicInfo';
import { Router } from '@angular/router';

let id = 0;

@Component({
  selector: 'app-basic-info-single',
  templateUrl: './basic-info-single.component.html',
  styleUrls: ['./basic-info-single.component.css']
})
export class BasicInfoSingleComponent implements OnInit {
  @Input() displayName: string;
  _basicInfoTotal: BasicInfoTotal;
  @Input() set basicInfoTotal(value: BasicInfoTotal) {
    this._basicInfoTotal = value;
  }
  @Input() statisticAccessor: string;
  @Input() clickLink: string;

  id: number;

  constructor(private router: Router) {
    this.id = id++;
  }

  ngOnInit(): void {

  }

  onClick() {
    this.router.navigateByUrl(this.clickLink);
  }

  getItems() {
    var items = {};
    for(let key in this._basicInfoTotal.basicInfoForSenders) {
      items[key] = this._basicInfoTotal.basicInfoForSenders[key][this.statisticAccessor];
    }

    return items;
  }
}
