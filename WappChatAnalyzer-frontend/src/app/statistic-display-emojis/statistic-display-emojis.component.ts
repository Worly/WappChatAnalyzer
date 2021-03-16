import { Component, OnInit } from '@angular/core';
import { EmojiInfoTotal } from '../dtos/emojiInfoTotal';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-statistic-display-emojis',
  templateUrl: './statistic-display-emojis.component.html',
  styleUrls: ['./statistic-display-emojis.component.css']
})
export class StatisticDisplayEmojisComponent implements OnInit {

  public emojis: EmojiInfoTotal;

  public showCount: number = 10;

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.dataService.getEmojiInfoTotal().subscribe(r => {
      console.log(r);
      this.emojis = r;
    });
  }

  showMoreClicked() {
    this.showCount += 10;
  }
}
