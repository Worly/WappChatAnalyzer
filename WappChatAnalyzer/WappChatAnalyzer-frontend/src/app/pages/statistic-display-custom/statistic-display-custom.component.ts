import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CustomStatistic } from '../../dtos/customStatistic';
import { StatisticService } from '../../services/statistic.service';
import { StatisticDisplayComponent } from '../statistic-display/statistic-display.component';

@Component({
  selector: 'app-statistic-display-custom',
  templateUrl: './statistic-display-custom.component.html',
  styleUrls: ['./statistic-display-custom.component.css']
})
export class StatisticDisplayCustomComponent implements OnInit {
  customStatistic: CustomStatistic;

  private subscriptions: Subscription[] = [];

  constructor(private route: ActivatedRoute, private statisticService: StatisticService) { }

  ngOnInit(): void {
    this.subscriptions.push(this.route.paramMap.subscribe(params => {
      let id = parseInt(params.get("id"));
      this.statisticService.getCustomStatistic(id).subscribe(s => {
        this.customStatistic = s
      });
    }));
  }

  ngOnDestroy() {
    while (this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }
}
