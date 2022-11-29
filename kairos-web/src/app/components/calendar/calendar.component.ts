import { Component, Renderer2 } from '@angular/core';
import { faPencil, faXmark } from '@fortawesome/free-solid-svg-icons';
import { CalendarService } from 'src/app/service/calendar.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent{
  faPencil: any = faPencil;
  faXmark: any = faXmark;

  constructor(public nav: NavbarService, public CalendarService: CalendarService, private renderer: Renderer2) { 
    this.nav.showBackButton();
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }
}
