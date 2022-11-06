import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TimerComponent } from './components/timer/timer.component';
import { LandingComponent } from './components/landing/landing.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { CalendarComponent } from './components/calendar/calendar.component';
 
const routes: Routes = [
  {
    path: 'timer',
    component: TimerComponent
  },
  {
    path: 'landing',
    component: LandingComponent
  },
  {
    path: 'statistics',
    component: StatisticsComponent
  },
  {
    path: 'calendar',
    component: CalendarComponent
  },
  {
    path: '',
    redirectTo: 'timer',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
