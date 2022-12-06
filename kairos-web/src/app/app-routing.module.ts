import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TimerComponent } from './components/timer/timer.component';
import { LandingComponent } from './components/landing/landing.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { EventEditComponent } from './components/event-edit/event-edit.component';
import { ErrorComponent } from './components/error/error.component';
import { EventDetailsComponent } from './components/event-details/event-details.component';
 
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
    component: CalendarComponent,
    children : 
    [
      { 
        path: ':eventId',
        component: EventDetailsComponent
      },
      {
        path: ':eventId/edit',
        component: EventEditComponent
      }
    ]
  },
  {
    path: '',
    redirectTo: 'timer',
    pathMatch: 'full'
  },
  {
    path: '**',
    component: ErrorComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
