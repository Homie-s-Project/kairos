import { Component, Renderer2 } from '@angular/core';
import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent{
  faXmark = faXmark;

  constructor(public nav: NavbarService, public CalendarService: CalendarService, private renderer: Renderer2) { 
    this.nav.showBackButton();
  }
}
