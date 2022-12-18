import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faCircleLeft, faCalendar, faChartLine, faPaw, faUser, faArrowRightFromBracket } from '@fortawesome/free-solid-svg-icons'
import { AuthService } from 'src/app/services/auth/auth.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent{
  faCircleLeft = faCircleLeft;
  faCalendar = faCalendar;
  faChartLine = faChartLine;
  faPaw = faPaw;
  faUser = faUser;
  faArrowRightFromBracket = faArrowRightFromBracket;

  constructor(public nav: NavbarService, public auth: AuthService, private _router: Router) { }

}
