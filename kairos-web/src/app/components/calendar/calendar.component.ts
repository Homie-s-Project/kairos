import { Component, Renderer2 } from '@angular/core';
import { faPencil, faXmark } from '@fortawesome/free-solid-svg-icons';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent{
  faPencil = faPencil;
  faXmark = faXmark;

  constructor(public nav: NavbarService, public CalendarService: CalendarService, private renderer: Renderer2, private _router: Router) { 
    this.nav.showBackButton();
  }

  goToEvent = (eventId: number|undefined) => {
    if (typeof eventId == undefined) {
      alert('L\'event ne peut pas être "undefined"')
      return 
    }

    this._router.navigate(['/calendar', eventId])
  }
}
