import { Component, OnInit } from '@angular/core';
import { faPencil, faXmark } from '@fortawesome/free-solid-svg-icons';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  faPencil = faPencil;
  faXmark = faXmark;

  constructor(public nav: NavbarService, public calendar: CalendarService) { 
    this.nav.showBackButton();
  }

  ngOnInit(): void {
  }

}
