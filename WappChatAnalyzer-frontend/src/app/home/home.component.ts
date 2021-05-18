import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BasicInfoTotal } from '../dtos/statisticTotal';
import { AfterAttach, BeforeDetach } from '../services/attach-detach-hooks.service';
import { StatisticService } from '../services/statistic.service';
import { FilterService } from '../services/filter.service';
import { CustomStatistic } from '../dtos/customStatistic';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy, AfterAttach, BeforeDetach {

  public basicInfoTotal: BasicInfoTotal;
  public customStatistics: CustomStatistic[];

  public isLoading: boolean;

  private subscriptions: Subscription[] = [];

  constructor(private statisticService: StatisticService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.subscribeAll();
    this.load();
  }

  ngOnDestroy() {
    this.unsubscribeAll();
  }

  ngAfterAttach() {
    this.subscribeAll();
    this.load();
  }

  ngBeforeDetach() {
    this.unsubscribeAll();
  }

  subscribeAll() {
    this.subscriptions.push(this.filterService.dateFilterChanged.subscribe(() => this.load()));
  }

  unsubscribeAll() {
    while(this.subscriptions.length > 0)
    this.subscriptions.pop().unsubscribe();
  }

  load() {
    this.isLoading = true;
    this.statisticService.getBasicInfo().subscribe(r => {
      this.isLoading = false;
      this.basicInfoTotal = r;
    });


    this.statisticService.getCustomStatistics().subscribe(r => {
      this.customStatistics = r;
      for(let stat of r) {
        this.statisticService.getCustomStatisticTotal(stat.id).subscribe(t => {
          stat["statisticTotal"] = t;
        })
      }
    });
  }

}
