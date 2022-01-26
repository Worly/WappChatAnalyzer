import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StatisticDisplayComponent } from './pages/statistic-display/statistic-display.component';
import { HomeComponent } from './pages/home/home.component';
import { StatisticTotalComponent } from './components/statistic-total/statistic-total.component';
import { EmojiComponent } from './components/emoji/emoji.component';
import { StatisticDisplayEmojisComponent } from './pages/statistic-display-emojis/statistic-display-emojis.component';
import { EmojiInfoSingleComponent } from './components/emoji-info-single/emoji-info-single.component';
import { RatioBarComponent } from './components/ratio-bar/ratio-bar.component';
import { LoadingComponent } from './components/loading/loading.component';
import { StatisticDisplaySingleEmojiComponent } from './pages/statistic-display-single-emoji/statistic-display-single-emoji.component';
import { Router, RouteReuseStrategy } from '@angular/router';
import { CustomReuseStrategy } from './services/route-reuse-strategy';
import { ToastComponent } from './components/toast/toast.component';
import { EventListComponent } from './pages/event-list/event-list.component';
import { OrderModule } from 'ngx-order-pipe';
import { EventEditComponent } from './pages/event-list/event-edit/event-edit.component';
import { EventGroupPickerComponent } from './pages/event-list/event-group-picker/event-group-picker.component';
import { EventGroupFilterComponent } from './components/event-group-filter/event-group-filter.component';
import { DateRangeFilterComponent } from './components/date-range-filter/date-range-filter.component';
import { FormsModule } from '@angular/forms';
import { DropdownComponent } from './components/dropdown/dropdown.component';
import { AttachDetachHooksService } from './services/attach-detach-hooks.service';
import { GroupingPeriodPickerComponent } from './components/grouping-period-picker/grouping-period-picker.component';
import { UploadChatExportComponent } from './pages/import/upload-chat-export/upload-chat-export.component';
import { ImportComponent } from './pages/import/import.component';
import { ChatComponent } from './pages/chat/chat.component';
import { InvokeDirective } from './directives/invoke-directive/invoke.directive';
import { StatisticDisplayCustomComponent } from './pages/statistic-display-custom/statistic-display-custom.component';
import { EventSearchComponent } from './components/event-search/event-search.component';
import { PerPickerComponent } from './components/per-picker/per-picker.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MatNativeDateModule } from '@angular/material/core';
import { CustomDateAdapter } from './services/custom-date-adapter';
import { AuthHttpInterceptor } from './services/auth/auth-http-interceptor.service';



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
    NoopAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: CustomReuseStrategy },
    {
      provide: APP_INITIALIZER, useFactory:
        function initAttachDetachHooks(router, reuseStrategy) {
          return () => new AttachDetachHooksService(router, reuseStrategy);
        },
      deps: [Router, RouteReuseStrategy], multi: true
    },
    { provide: DateAdapter, useClass: CustomDateAdapter },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

