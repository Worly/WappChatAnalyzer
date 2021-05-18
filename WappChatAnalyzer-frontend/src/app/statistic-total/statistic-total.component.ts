import { Component, Input, OnInit } from '@angular/core';
import { StatisticTotal } from '../dtos/statisticTotal';
import { Router } from '@angular/router';

@Component({
  selector: 'app-statistic-total',
  templateUrl: './statistic-total.component.html',
  styleUrls: ['./statistic-total.component.css']
})
export class StatisticTotalComponent implements OnInit {
  @Input() displayName: string;
  _statisticTotal: StatisticTotal;
  @Input() set statisticTotal(value: StatisticTotal) {
    this._statisticTotal = value;
  }
  @Input() clickLink: string;

  id: number;

  constructor(private router: Router) {
  }

  ngOnInit(): void {

  }

  onClick() {
    this.router.navigateByUrl(this.clickLink);
  }
}
