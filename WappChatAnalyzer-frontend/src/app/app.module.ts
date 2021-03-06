import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StatisticDisplayComponent } from './statistic-display/statistic-display.component';
import { HomeComponent } from './home/home.component';
import { StatisticTotalComponent } from './statistic-total/statistic-total.component';
import { EmojiComponent } from './emoji/emoji.component';
import { StatisticDisplayEmojisComponent } from './statistic-display-emojis/statistic-display-emojis.component';
import { EmojiInfoSingleComponent } from './emoji-info-single/emoji-info-single.component';
import { RatioBarComponent } from './ratio-bar/ratio-bar.component';
import { LoadingComponent } from './loading/loading.component';
import { StatisticDisplaySingleEmojiComponent } from './statistic-display-single-emoji/statistic-display-single-emoji.component';
import { Router, RouteReuseStrategy } from '@angular/router';
import { CustomReuseStrategy } from './services/route-reuse-strategy';
import { ToastComponent } from './toast/toast.component';
import { EventListComponent } from './event-list/event-list.component';
import { OrderModule } from 'ngx-order-pipe';
import { EventEditComponent } from './event-list/event-edit/event-edit.component';
import { DpDatePickerModule } from 'ng2-date-picker';
import { EventGroupPickerComponent } from './event-list/event-group-picker/event-group-picker.component';
import { EventGroupFilterComponent } from './event-group-filter/event-group-filter.component';
import { DateRangeFilterComponent } from './date-range-filter/date-range-filter.component';
import { FormsModule } from '@angular/forms';
import { DropdownComponent } from './dropdown/dropdown.component';
import { AttachDetachHooksService } from './services/attach-detach-hooks.service';
import { GroupingPeriodPickerComponent } from './grouping-period-picker/grouping-period-picker.component';
import { UploadChatExportComponent } from './upload-chat-export/upload-chat-export.component';
import { ImportComponent } from './import/import.component';
import { ChatComponent } from './chat/chat.component';
import { InvokeDirective } from './invoke-directive/invoke.directive';
import { StatisticDisplayCustomComponent } from './statistic-display-custom/statistic-display-custom.component';
import { EventSearchComponent } from './event-search/event-search.component';
import { PerPickerComponent } from './per-picker/per-picker.component';



@NgModule({
  declarations: [
    AppComponent,
    InvokeDirective,
    StatisticDisplayComponent,
    HomeComponent,
    StatisticTotalComponent,
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
    EventGroupFilterComponent,
    DateRangeFilterComponent,
    DropdownComponent,
    GroupingPeriodPickerComponent,
    UploadChatExportComponent,
    ImportComponent,
    ChatComponent,
    StatisticDisplayCustomComponent,
    EventSearchComponent,
    PerPickerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    OrderModule,
    DpDatePickerModule
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: CustomReuseStrategy },
    {
      provide: APP_INITIALIZER, useFactory:
        function initAttachDetachHooks(router, reuseStrategy) {
          return () => new AttachDetachHooksService(router, reuseStrategy);
        },
      deps: [Router, RouteReuseStrategy], multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

