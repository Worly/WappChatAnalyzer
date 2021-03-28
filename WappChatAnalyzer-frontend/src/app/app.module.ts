import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StatisticDisplayComponent } from './statistic-display/statistic-display.component';
import { HomeComponent } from './home/home.component';
import { BasicInfoSingleComponent } from './basic-info-single/basic-info-single.component';
import { EmojiComponent } from './emoji/emoji.component';
import { StatisticDisplayEmojisComponent } from './statistic-display-emojis/statistic-display-emojis.component';
import { EmojiInfoSingleComponent } from './emoji-info-single/emoji-info-single.component';
import { RatioBarComponent } from './ratio-bar/ratio-bar.component';
import { LoadingComponent } from './loading/loading.component';
import { StatisticDisplaySingleEmojiComponent } from './statistic-display-single-emoji/statistic-display-single-emoji.component';
import { RouteReuseStrategy } from '@angular/router';
import { CustomReuseStrategy } from './services/route-reuse-strategy';
import { ToastComponent } from './toast/toast.component';
import { EventListComponent } from './event-list/event-list.component';
import { OrderModule } from 'ngx-order-pipe';
import { EventEditComponent } from './event-list/event-edit/event-edit.component';
import { DpDatePickerModule } from 'ng2-date-picker';
import { EventGroupPickerComponent } from './event-list/event-group-picker/event-group-picker.component';
import { EventGroupFilterComponent } from './event-group-filter/event-group-filter.component';

@NgModule({
  declarations: [
    AppComponent,
    StatisticDisplayComponent,
    HomeComponent,
    BasicInfoSingleComponent,
    EmojiComponent,
    StatisticDisplayEmojisComponent,
    EmojiInfoSingleComponent,
    RatioBarComponent,
    LoadingComponent,
    StatisticDisplaySingleEmojiComponent,
    ToastComponent,
    EventListComponent,
    EventEditComponent,
    EventGroupPickerComponent,
    EventGroupFilterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    OrderModule,
    DpDatePickerModule
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: CustomReuseStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
