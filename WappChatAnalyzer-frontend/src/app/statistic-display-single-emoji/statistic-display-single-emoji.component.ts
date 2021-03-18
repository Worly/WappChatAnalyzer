import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-statistic-display-single-emoji',
  templateUrl: './statistic-display-single-emoji.component.html',
  styleUrls: ['./statistic-display-single-emoji.component.css']
})
export class StatisticDisplaySingleEmojiComponent implements OnInit {

  statisticUrl: string;
  codePoints: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.statisticUrl = "emoji/getStatistic/singleEmoji/" + params.get("codePoints");
      this.codePoints = params.get("codePoints");
    });
  }

}
