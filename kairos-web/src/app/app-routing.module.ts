import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedGuard } from './guards/isLogged/is-logged.guard';
import { TimerComponent } from './components/timer/timer.component';
import { LandingComponent } from './components/landing/landing.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { ErrorComponent } from './components/error/error.component';
import { EventEditComponent } from './components/event-edit/event-edit.component';
import {ProfileComponent} from "./components/profile/profile.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: '/timer',
    pathMatch: 'full',
  },
  {
    path: '',
    canActivateChild: [IsLoggedGuard],
    children: [
      {
        path: 'timer',
        component: TimerComponent
      },
      {
        path: 'statistics',
        component: StatisticsComponent
      },
      {
        path: 'calendar',
        component: CalendarComponent,
        children : [
          { path: ':eventId',
          component: EventEditComponent } 
        ]
      },
      {
        path:'profile',
        component: ProfileComponent
      },
    ]
  },
  {
    path: 'landing',
    component: LandingComponent
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
