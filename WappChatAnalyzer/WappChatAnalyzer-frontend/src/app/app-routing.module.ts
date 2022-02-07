import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './pages/chat/chat.component';
import { EventListComponent } from './pages/event-list/event-list.component';
import { HomeComponent } from './pages/home/home.component';
import { ImportComponent } from './pages/import/import.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { StatisticDisplayCustomComponent } from './pages/statistic-display-custom/statistic-display-custom.component';
import { StatisticDisplayEmojisComponent } from './pages/statistic-display-emojis/statistic-display-emojis.component';
import { StatisticDisplaySingleEmojiComponent } from './pages/statistic-display-single-emoji/statistic-display-single-emoji.component';
import { StatisticDisplayComponent } from './pages/statistic-display/statistic-display.component';

const routes: Routes = [
  { path: "login", component: LoginComponent },
  { path: "register", component: RegisterComponent },
  { path: "home", component: HomeComponent },
  { path: "statistic-display/emoji/:codePoints", component: StatisticDisplaySingleEmojiComponent },
  { path: "statistic-display/messages", component: StatisticDisplayComponent, data: { statisticUrl: "statistic/getStatistic/numberOfMessages", displayName: "Number of messages" } },
  { path: "statistic-display/words", component: StatisticDisplayComponent, data: { statisticUrl: "statistic/getStatistic/numberOfWords", displayName: "Number of words" } },
  { path: "statistic-display/characters", component: StatisticDisplayComponent, data: { statisticUrl: "statistic/getStatistic/numberOfCharacters", displayName: "Number of characters" } },
  { path: "statistic-display/media", component: StatisticDisplayComponent, data: { statisticUrl: "statistic/getStatistic/numberOfMedia", displayName: "Number of media" } },
  { path: "statistic-display/emojis", component: StatisticDisplayEmojisComponent, data: { shouldDetach: true } },
  { path: "statistic-display/custom/:id", component: StatisticDisplayCustomComponent },
  { path: "events", component: EventListComponent },
  { path: "import", component: ImportComponent },
  { path: "chat", component: ChatComponent },
  { path: "**", redirectTo: "/home" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
