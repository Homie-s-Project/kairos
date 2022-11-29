import { Component, OnInit, Renderer2 } from '@angular/core';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {

  constructor(public nav: NavbarService, private renderer: Renderer2) {
    this.nav.showBackButton();
  }

  ngOnInit(): void {
  }

}
