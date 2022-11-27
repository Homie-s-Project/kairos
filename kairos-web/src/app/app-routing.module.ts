import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedGuard } from './guards/isLogged/is-logged.guard';
import { TimerComponent } from './components/timer/timer.component';
import { LandingComponent } from './components/landing/landing.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { ErrorComponent } from './components/error/error.component';
import { EventEditComponent } from './components/event-edit/event-edit.component';

 
const routes: Routes = [
  {
    path: 'timer',
    component: TimerComponent,
    canActivate: [IsLoggedGuard]
  },
  {
    path: 'landing',
    component: LandingComponent
  },
  {
    path: 'statistics',
    component: StatisticsComponent,
    canActivate: [IsLoggedGuard]
  },
  {
    path: 'calendar',
    component: CalendarComponent,
    children : [
      { path: ':eventId',
      component: EventEditComponent } 
    ],
    canActivate: [IsLoggedGuard]
  },
  {
    path: '',
    redirectTo: 'timer',
    pathMatch: 'full' 
  },
  {
    path: '**',
    component: ErrorComponent,
    canActivate: [IsLoggedGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
