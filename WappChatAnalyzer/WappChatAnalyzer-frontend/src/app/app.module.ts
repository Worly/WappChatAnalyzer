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
import { LoginComponent } from './pages/login/login.component';
import { ErrorTranslateInterceptor } from './services/errors/error-translate.service';
import { RegisterComponent } from './pages/register/register.component';
import { ErrorComponent } from './pages/error/error.component';
import { ErrorInterceptor } from './services/errors/error-interceptor.service';
import { WorkspacesComponent } from './pages/workspaces/workspaces.component';
import { SingleWorkspaceComponent } from './pages/workspaces/single-workspace/single-workspace.component';
import { AppInitializerService } from './services/app-initializer.service';
import { ContextMenuComponent } from './components/context-menu/context-menu.component';
import { ButtonComponent } from './components/button/button.component';
import { ToppyModule } from 'toppy';
import { ElementRefDirective } from './directives/element-ref.directive';
import { FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { DialogComponent } from './components/dialog/dialog.component';
import { WorkspaceEditComponent } from './pages/workspaces/single-workspace/workspace-edit/workspace-edit.component';
import { SelectedWorkspaceComponent } from './components/selected-workspace/selected-workspace.component';
import { WorkspaceInterceptor } from './services/workspaces/workspace-interceptor.service';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    WorkspacesComponent,
    ErrorComponent,
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
    PerPickerComponent,
    SingleWorkspaceComponent,
    ContextMenuComponent,
    ButtonComponent,
    ElementRefDirective,
    DialogComponent,
    WorkspaceEditComponent,
    SelectedWorkspaceComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    OrderModule,
    NoopAnimationsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ToppyModule,
    FontAwesomeModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    })
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
    { provide: APP_INITIALIZER, useFactory: appInitializerFactory, deps: [AppInitializerService], multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: WorkspaceInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorTranslateInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(library: FaIconLibrary) {
    library.addIconPacks(fas);
  }

}

export function appInitializerFactory(appInitializerService: AppInitializerService) {
  return () => appInitializerService.initialize();
}

