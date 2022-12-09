import {HttpClientModule} from "@angular/common/http"
import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {NgChartsModule} from 'ng2-charts';
import {CookieService} from 'ngx-cookie-service'
import {ShareButtonModule} from "ngx-sharebuttons/button";

import {ErrorComponent} from './components/error/error.component';
import {ErrorNotFoundComponent} from './components/error-not-found/error-not-found.component';
import {EventListComponent} from './components/event-list/event-list.component';
import {EventCardComponent} from './components/event-card/event-card.component';
import {EventEditComponent} from './components/event-edit/event-edit.component';
import {TinyTimerComponent} from './components/tiny-timer/tiny-timer.component';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {CalendarComponent} from './components/calendar/calendar.component';
import {LandingComponent} from './components/landing/landing.component';
import {TimerComponent} from './components/timer/timer.component';
import {StatisticsComponent} from './components/statistics/statistics.component';
import {NavbarComponent} from './components/navbar/navbar.component';
import {ProfileComponent} from './components/profile/profile.component';

@NgModule({
  declarations: [
    AppComponent,
    CalendarComponent,
    LandingComponent,
    TimerComponent,
    StatisticsComponent,
    NavbarComponent,
    ErrorComponent,
    EventCardComponent,
    EventListComponent,
    ErrorNotFoundComponent,
    EventEditComponent,
    TinyTimerComponent,
    NavbarComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ShareButtonModule,
    FontAwesomeModule,
    NgChartsModule
  ],
  providers: [
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
