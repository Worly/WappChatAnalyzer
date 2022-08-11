import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './pages/chat/chat.component';
import { ErrorComponent } from './pages/error/error.component';
import { EventListComponent } from './pages/event-list/event-list.component';
import { HomeComponent } from './pages/home/home.component';
import { ImportComponent } from './pages/import/import.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { StatisticDisplayCustomComponent } from './pages/statistic-display-custom/statistic-display-custom.component';
import { StatisticDisplayEmojisComponent } from './pages/statistic-display-emojis/statistic-display-emojis.component';
import { StatisticDisplaySingleEmojiComponent } from './pages/statistic-display-single-emoji/statistic-display-single-emoji.component';
import { StatisticDisplayComponent } from './pages/statistic-display/statistic-display.component';
import { VerifyComponent } from './pages/verify/verify.component';
import { WorkspacesComponent } from './pages/workspaces/workspaces.component';
import { LoggedInGuard, NotLoggedInGuard } from './services/auth/auth.guard';
import { SelectedWorkspaceGuard } from './services/workspaces/workspace.guard';

const routes: Routes = [
  { path: "login", component: LoginComponent, canActivate: [NotLoggedInGuard] },
  { path: "register", component: RegisterComponent, canActivate: [NotLoggedInGuard] },

  {
    path: "workspaces",
    component: WorkspacesComponent,
    canActivate: [LoggedInGuard]
  },
  {
    path: "home",
    component: HomeComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  {
    path: "statistic-display/emoji/:codePoints",
    component: StatisticDisplaySingleEmojiComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  {
    path: "statistic-display/messages",
    component: StatisticDisplayComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard],
    data: {
      statisticUrl: "statistic/getStatistic/numberOfMessages",
      displayName: "Number of messages"
    }
  },
  {
    path: "statistic-display/words",
    component: StatisticDisplayComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard],
    data: {
      statisticUrl: "statistic/getStatistic/numberOfWords",
      displayName: "Number of words"
    }
  },
  {
    path: "statistic-display/characters",
    component: StatisticDisplayComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard],
    data: {
      statisticUrl: "statistic/getStatistic/numberOfCharacters",
      displayName: "Number of characters"
    }
  },
  {
    path: "statistic-display/media",
    component: StatisticDisplayComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard],
    data: {
      statisticUrl: "statistic/getStatistic/numberOfMedia",
      displayName: "Number of media"
    }
  },
  {
    path: "statistic-display/emojis",
    component: StatisticDisplayEmojisComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard],
    data: {
      shouldDetach: true
    }
  },
  {
    path: "statistic-display/custom/:id",
    component: StatisticDisplayCustomComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  {
    path: "events",
    component: EventListComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  {
    path: "import",
    component: ImportComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  {
    path: "chat",
    component: ChatComponent,
    canActivate: [LoggedInGuard, SelectedWorkspaceGuard]
  },
  { 
    path: "verify",
    component: VerifyComponent
  },
  {
    path: "error",
    component: ErrorComponent,
  },
  { path: "**", redirectTo: "/home" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
