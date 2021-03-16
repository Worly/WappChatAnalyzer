import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { StatisticDisplayEmojisComponent } from './statistic-display-emojis/statistic-display-emojis.component';
import { StatisticDisplayComponent } from './statistic-display/statistic-display.component';

const routes: Routes = [
  { path: "home", component: HomeComponent },
  { path: "statistic-display/NumberOfEmojis", component: StatisticDisplayEmojisComponent },
  { path: "statistic-display/:statisticName", component: StatisticDisplayComponent },
  { path: "**", redirectTo: "/home" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
