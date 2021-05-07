import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-statistic-display-single-emoji',
  templateUrl: './statistic-display-single-emoji.component.html',
  styleUrls: ['./statistic-display-single-emoji.component.css']
})
export class StatisticDisplaySingleEmojiComponent implements OnInit, OnDestroy {

  statisticUrl: string;
  codePoints: string;

  private subscriptions: Subscription[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.subscriptions.push(this.route.paramMap.subscribe(params => {
      this.statisticUrl = "emoji/getStatistic/singleEmoji/" + params.get("codePoints");
      this.codePoints = params.get("codePoints");
    }));
  }

  ngOnDestroy() {
    while(this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

}
