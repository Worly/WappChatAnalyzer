import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { StatisticTotal } from '../dtos/statisticTotal';
import { Router } from '@angular/router';
import { FilterService, FilterType } from '../services/filter.service';
import { Subscription, Unsubscribable } from 'rxjs';
import { StatisticService } from '../services/statistic.service';

@Component({
  selector: 'app-statistic-total',
  templateUrl: './statistic-total.component.html',
  styleUrls: ['./statistic-total.component.css']
})
export class StatisticTotalComponent implements OnInit, OnDestroy {
  @Input() displayName: string;
  @Input() statisticTotalApiUrl: string;
  @Input() clickLink: string;

  statisticTotal: StatisticTotal;

  id: number;

  private subscriptions: Unsubscribable[] = [];

  constructor(private statisticService: StatisticService, private filterService: FilterService, private router: Router) {
  }

  ngOnInit(): void {
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.DATE_RANGE], () => this.load()));

    this.load();
  }

  ngOnDestroy() {
    while (this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  private load() {
    this.statisticTotal = null;
    this.statisticService.getStatisticTotal(this.statisticTotalApiUrl).subscribe(o => {
      this.statisticTotal = o;
    });
  }

  onClick() {
    this.router.navigateByUrl(this.clickLink);
  }
}
