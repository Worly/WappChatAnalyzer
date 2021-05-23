import { Component, OnInit, ViewChild } from '@angular/core';
import { Subscription, Unsubscribable } from 'rxjs';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { AfterAttach, BeforeDetach } from '../services/attach-detach-hooks.service';
import { StatisticService } from '../services/statistic.service';
import { FilterService, FilterType } from '../services/filter.service';
import { StatisticDisplayComponent } from '../statistic-display/statistic-display.component';

@Component({
  selector: 'app-statistic-display-emojis',
  templateUrl: './statistic-display-emojis.component.html',
  styleUrls: ['./statistic-display-emojis.component.css']
})
export class StatisticDisplayEmojisComponent implements OnInit, AfterAttach, BeforeDetach {

  @ViewChild(StatisticDisplayComponent)
  statisticDisplay: StatisticDisplayComponent;

  public emojis: EmojiInfoTotal;

  public showCount: number = 10;

  private subscriptions: Unsubscribable[] = [];

  constructor(private statisticService: StatisticService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.subscribeAll();
    this.load();
  }

  ngAfterAttach() {
    this.subscribeAll();
    this.load();
    this.statisticDisplay.ngAfterAttach();
  }

  ngBeforeDetach() {
    this.unsubscribeAll();
    this.statisticDisplay.ngBeforeDetach();
  }

  subscribeAll() {
    this.subscriptions.push(this.filterService.subscribeToFilterChanged([FilterType.DATE_RANGE], () => this.load()));
  }

  unsubscribeAll() {
    while(this.subscriptions.length > 0)
      this.subscriptions.pop().unsubscribe();
  }

  load() {
    this.statisticService.getEmojiInfoTotal().subscribe(r => {
      this.emojis = r;
    });
  }

  showMoreClicked() {
    this.showCount += 10;
  }
}
