import {Component, OnInit} from '@angular/core';
import {NavbarService} from 'src/app/services/navbar/navbar.service';
import calendar from 'calendar-js'
import {Router} from "@angular/router";

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {

  private selectedMonth?: number;
  private selectedYear?: number;
  private selectedDay?: number;

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

  private todayday = new Date().getUTCDate();
  private todayMonth = new Date().getUTCMonth();
  private todayYear = new Date().getFullYear();

  public previousCalendarMonth?: CalendarType;
  public currentCalendarMonth?: CalendarType;
  public nextCalendarMonth?: CalendarType;

  public currentMonthName?: string;
  public currentYearName?: string;

  public isLoadingCalendar: boolean = true;
  public sidePanelOpen: boolean = true;

  constructor(public nav: NavbarService, private router: Router) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
    this.updateCalendar(new Date().getMonth(), new Date().getFullYear());
  }

  prevMonthCalendar() {
    if (this.selectedMonth === undefined){
      this.selectedMonth = new Date().getMonth();
    }

    if (this.selectedYear === undefined){
      this.selectedYear = new Date().getFullYear();
    }

    let previousMonth = this.selectedMonth == 0 ? 11 : this.selectedMonth - 1;
    let previousYear = this.selectedMonth == 0 ? this.selectedYear - 1 : this.selectedYear;

    this.updateCalendar(previousMonth, previousYear);
  }

  currentMonthCalendar() {
    if (this.selectedMonth === undefined){
      this.selectedMonth = new Date().getMonth();
    }

    if (this.selectedYear === undefined){
      this.selectedYear = new Date().getFullYear();
    }

    this.updateCalendar(new Date().getMonth(), new Date().getFullYear());
  }

  nextMonthCalendar() {
    if (this.selectedMonth === undefined){
      this.selectedMonth = new Date().getMonth();
    }

    if (this.selectedYear === undefined){
      this.selectedYear = new Date().getFullYear();
    }

    let nextMonth = this.selectedMonth == 11 ? 0 : this.selectedMonth + 1;
    let nextYear = this.selectedMonth == 11 ? this.selectedYear + 1 : this.selectedYear;

    this.updateCalendar(nextMonth, nextYear);
  }

  updateCalendar(month: number, year: number) {
    this.isLoadingCalendar = true;

    let date = new Date();
    date.setDate(1);
    date.setMonth(month);
    date.setFullYear(year);

    this.selectedMonth = date.getMonth();
    let previousMonth = this.selectedMonth == 0 ? 11 : this.selectedMonth - 1;
    let nextMonth = this.selectedMonth == 11 ? 0 : this.selectedMonth + 1;

    this.selectedYear = date.getFullYear();
    let previousYear = this.selectedMonth == 0 ? this.selectedYear - 1 : this.selectedYear;
    let nextYear = this.selectedMonth == 11 ? this.selectedYear + 1 : this.selectedYear;

    this.previousCalendarMonth = this.calendar.of(previousYear, previousMonth);
    this.currentCalendarMonth = this.calendar.of(this.selectedYear, this.selectedMonth);
    this.nextCalendarMonth = this.calendar.of(nextYear, nextMonth);

    this.currentMonthName = this.currentCalendarMonth?.month;
    this.currentYearName = this.currentCalendarMonth?.year.toString();

    // Ajouter des dates avant le mois courant
    this.currentCalendarMonth.calendar[0] = this.previousCalendarMonth.calendar[this.previousCalendarMonth.calendar.length -1]
      .slice(0, this.currentCalendarMonth.firstWeekday)
      .concat(this.currentCalendarMonth.calendar[0]
        .slice(this.currentCalendarMonth.firstWeekday, this.currentCalendarMonth.calendar[0].length))

    // Ajouter des dates après le mois courant seulement si la dernière semaine n'est pas complète
    // On vérifie ça en regardant si le jour du départ du mois suivant n'est pas lundi
    if (this.nextCalendarMonth.firstWeekday > 0) {
      this.currentCalendarMonth.calendar[this.currentCalendarMonth.calendar.length -1] = this.currentCalendarMonth.calendar[this.currentCalendarMonth.calendar.length -1]
        .slice(0, this.nextCalendarMonth.firstWeekday)
        .concat(this.nextCalendarMonth.calendar[0]
          .slice(this.nextCalendarMonth.firstWeekday, this.nextCalendarMonth.calendar[0].length));
    }

    // TODO: Le mettre après la récupération des données
    this.isLoadingCalendar = false;
  }

  isToday(day: number): boolean {
    return this.selectedMonth == this.todayMonth && this.selectedYear == this.todayYear && day == this.todayday;
  }

  isOtherMonth(day: number, daysOfWeeks: number[]): boolean {
    let firstArrayOfCurrentCalendar = this.currentCalendarMonth?.calendar[0];
    let lastArrayOfCurrentCalendar = this.currentCalendarMonth?.calendar[this.currentCalendarMonth?.calendar.length -1];

    if (!this.nextCalendarMonth || !this.previousCalendarMonth || !this.currentCalendarMonth || !firstArrayOfCurrentCalendar || !lastArrayOfCurrentCalendar) {
      console.warn("Erreur dans le chargement du calendrier");
      return false;
    }

    // Détermine si le jour est dans le mois précédent
    if (firstArrayOfCurrentCalendar.indexOf(day) !== -1 &&
      firstArrayOfCurrentCalendar.indexOf(day) < this.currentCalendarMonth.firstWeekday &&
      daysOfWeeks[daysOfWeeks.length -1] <= (7 - this.currentCalendarMonth.firstWeekday) &&
      daysOfWeeks[this.previousCalendarMonth?.lastWeekday +1] === 1) {
      return true;
    }

    // Détermine si le jour est dans le mois suivant
    if (lastArrayOfCurrentCalendar.indexOf(day) !== -1 &&
      lastArrayOfCurrentCalendar.indexOf(day) > this.currentCalendarMonth?.lastWeekday &&
      daysOfWeeks[0] > 1 &&
      daysOfWeeks[this.nextCalendarMonth?.firstWeekday -1 ] === this.currentCalendarMonth.calendar[this.currentCalendarMonth.calendar.length -1][this.currentCalendarMonth.lastWeekday]) {
      return true;
    }

    return false;
  }

  selectDay(day: number) {
    this.selectedDay = day;
  }

  isSelectedDay(date: number, daysOfWeek: number[]) {
    return this.selectedDay === date && daysOfWeek.includes(date);
  }
}
