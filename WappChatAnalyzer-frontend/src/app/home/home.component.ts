import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AfterAttach, BeforeDetach } from '../services/attach-detach-hooks.service';
import { StatisticService } from '../services/statistic.service';
import { FilterService } from '../services/filter.service';
import { CustomStatistic } from '../dtos/customStatistic';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public customStatistics: CustomStatistic[];

  public isLoading: boolean;

  constructor(private statisticService: StatisticService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.statisticService.getCustomStatistics().subscribe(r => {
      this.customStatistics = r;
    });
  }

}
