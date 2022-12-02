import {Component, OnInit} from '@angular/core';
import {NavbarService} from 'src/app/services/navbar/navbar.service';
import { Calendar } from 'calendar-base';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {

  private currentMonth = new Date().getMonth();
  private currentYear = new Date().getFullYear();

  private calendar = new Calendar({
    startDate: {
      day: 1,
      month: 0,
      year: this.currentYear
    },
    endDate: {
      day: 31,
      month: 11,
      year: this.currentYear
    },
    weekNumbers: true,
    weekStart: 1,
    siblingMonths: true
  });

  constructor(public nav: NavbarService) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
    var calendar = this.calendar.getCalendar(this.currentYear, this.currentMonth);

    console.log(calendar);
  }

}
