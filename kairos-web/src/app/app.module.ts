import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {NgChartsModule} from 'ng2-charts';
import {ShareButtonModule} from "ngx-sharebuttons/button";

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {CalendarComponent} from './components/calendar/calendar.component';
import {LandingComponent} from './components/landing/landing.component';
import {TimerComponent} from './components/timer/timer.component';
import {StatisticsComponent} from './components/statistics/statistics.component';
import {NavbarComponent} from './components/navbar/navbar.component';
import {EventCardComponent} from './components/event-card/event-card.component';
import {EventListComponent} from './components/event-list/event-list.component';
import {ErrorNotFoundComponent} from './components/error-not-found/error-not-found.component';
import {EventEditComponent} from './components/event-edit/event-edit.component';
import {ErrorComponent} from './components/error/error.component';
import {TinyTimerComponent} from './components/tiny-timer/tiny-timer.component';
import {EventDetailsComponent} from './components/event-details/event-details.component';


@NgModule({
  declarations: [
    AppComponent,
    CalendarComponent,
    LandingComponent,
    TimerComponent,
    StatisticsComponent,
    NavbarComponent,
    EventCardComponent,
    EventListComponent,
    ErrorNotFoundComponent,
    EventEditComponent,
    ErrorComponent,
    TinyTimerComponent,
    EventDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ShareButtonModule,
    FontAwesomeModule,
    NgChartsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
