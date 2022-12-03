import {Component, OnInit} from '@angular/core';
import {NavbarService} from 'src/app/services/navbar/navbar.service';
import calendar from 'calendar-js'

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {

  private currentMonth?: number;
  private previousMonth?: number;
  private nextMonth?: number;

  private currentYear?: number;
  private previousYear?: number;
  private nextYear?: number;


  private calendar = calendar({
    months: [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December'
    ],
    monthsAbbr: [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec'
    ],
  });

  public previousCalendarMonth?: CalendarType;
  public currentCalendarMonth?: CalendarType;
  public nextCalendarMonth?: CalendarType;

  public currentMonthName?: string;

  constructor(public nav: NavbarService) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
    this.updateCalendar(new Date().getMonth(), new Date().getFullYear());

    console.log(this.previousCalendarMonth, this.currentCalendarMonth, this.nextCalendarMonth)
  }

  prevMonthCalendar() {
    let previousNumber = this.currentMonth == 0 ? 11 : this.currentMonth - 1;
    let previousYear = this.currentMonth == 0 ? this.currentYear - 1 : this.currentYear;

    this.updateCalendar(previousNumber, previousYear);
  }

  currentMonthCalendar() {
    this.updateCalendar(new Date().getMonth(), new Date().getFullYear());
  }

  nextMonthCalendar() {
    let nextNumber = this.currentMonth == 11 ? 0 : this.currentMonth + 1;
    let nextYear = this.currentMonth == 11 ? this.currentYear + 1 : this.currentYear;

    this.updateCalendar(nextNumber, nextYear);
  }

  updateCalendar(month: number, year: number) {
    console.log(month, year)

    let date = new Date();
    date.setMonth(month);
    date.setFullYear(year);

    this.currentMonth = date.getMonth();
    this.previousMonth = this.currentMonth == 0 ? 11 : this.currentMonth - 1;
    this.nextMonth = this.currentMonth == 11 ? 0 : this.currentMonth + 1;

    this.currentYear = new Date().getFullYear();
    this.previousYear = this.currentMonth == 0 ? this.currentYear - 1 : this.currentYear;
    this.nextYear = this.currentMonth == 11 ? this.currentYear + 1 : this.currentYear;

    this.previousCalendarMonth = this.calendar.of(this.previousYear, this.previousMonth);
    this.currentCalendarMonth = this.calendar.of(this.currentYear, this.currentMonth);
    this.nextCalendarMonth = this.calendar.of(this.nextYear, this.nextMonth);

    this.currentMonthName = this.currentCalendarMonth?.month;
  }
}
