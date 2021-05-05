import { Component, OnInit, ViewChild } from '@angular/core';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { AfterAttach } from '../services/attach-detach-hooks.service';
import { DataService } from '../services/data.service';
import { FilterService } from '../services/filter.service';
import { StatisticDisplayComponent } from '../statistic-display/statistic-display.component';

@Component({
  selector: 'app-statistic-display-emojis',
  templateUrl: './statistic-display-emojis.component.html',
  styleUrls: ['./statistic-display-emojis.component.css']
})
export class StatisticDisplayEmojisComponent implements OnInit, AfterAttach {

  @ViewChild(StatisticDisplayComponent)
  statisticDisplay: StatisticDisplayComponent;

  public emojis: EmojiInfoTotal;

  public showCount: number = 10;

  constructor(private dataService: DataService, private filterService: FilterService) { }

  ngOnInit(): void {
    this.filterService.dateFilterChanged.subscribe(() => this.load());
    this.filterService.groupingPeriodChanged.subscribe(() => this.load());
    this.load();
  }

  ngAfterAttach() {
    this.statisticDisplay.ngAfterAttach();
  }

  load() {
    this.dataService.getEmojiInfoTotal().subscribe(r => {
      this.emojis = r;
    });
  }

  showMoreClicked() {
    this.showCount += 10;
  }
}
