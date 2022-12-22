import {Component, OnInit} from '@angular/core';
import {NavbarService} from 'src/app/services/navbar/navbar.service';
import calendar from 'calendar-js'
import {ActivatedRoute, Router} from "@angular/router";
import {EventService} from "../../services/event/event.service";
import {IGroupModel} from "../../models/IGroupModel";

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

  public groups?: IGroupModel[];

  constructor(public nav: NavbarService,
              private eventService: EventService,
              private router: Router,
              private route: ActivatedRoute) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
  }

}
