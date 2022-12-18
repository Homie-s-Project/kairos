import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgChartsModule } from 'ng2-charts';
import {ShareButtonModule} from "ngx-sharebuttons/button";

import {ErrorComponent} from './components/error/error.component';
import { TinyTimerComponent } from './components/tiny-timer/tiny-timer.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { LandingComponent } from './components/landing/landing.component';
import { TimerComponent } from './components/timer/timer.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HttpClientModule } from '@angular/common/http';
@NgModule({
  declarations: [
    AppComponent,
    CalendarComponent,
    LandingComponent,
    TimerComponent,
    StatisticsComponent,
    NavbarComponent,
    ErrorComponent,
    TinyTimerComponent,
    NavbarComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ShareButtonModule,
    FontAwesomeModule,
    NgChartsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
