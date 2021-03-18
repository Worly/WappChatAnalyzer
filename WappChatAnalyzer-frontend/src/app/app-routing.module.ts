import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StatisticDisplayEmojisComponent } from './statistic-display-emojis/statistic-display-emojis.component';
import { StatisticDisplaySingleEmojiComponent } from './statistic-display-single-emoji/statistic-display-single-emoji.component';
import { StatisticDisplayComponent } from './statistic-display/statistic-display.component';

const routes: Routes = [
  { path: "home", component: HomeComponent },
  { path: "statistic-display/emoji/:codePoints", component: StatisticDisplaySingleEmojiComponent },
  { path: "statistic-display/messages", component: StatisticDisplayComponent, data: { statisticUrl: "basic/getStatistic/numberOfMessages", displayName: "Number of messages" } },
  { path: "statistic-display/words", component: StatisticDisplayComponent, data: { statisticUrl: "basic/getStatistic/numberOfWords", displayName: "Number of words" } },
  { path: "statistic-display/characters", component: StatisticDisplayComponent, data: { statisticUrl: "basic/getStatistic/numberOfCharacters", displayName: "Number of characters" } },
  { path: "statistic-display/media", component: StatisticDisplayComponent, data: { statisticUrl: "basic/getStatistic/numberOfMedia", displayName: "Number of media" } },
  { path: "statistic-display/emojis", component: StatisticDisplayEmojisComponent },
  { path: "**", redirectTo: "/home" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
