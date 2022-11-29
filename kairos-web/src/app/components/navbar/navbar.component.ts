import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faCircleLeft, faCalendar, faChartLine, faPaw, faUser, faEllipsis } from '@fortawesome/free-solid-svg-icons'
import { NavbarService } from 'src/app/services/navbar/navbar.service';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent{
  faCircleLeft: any = faCircleLeft;
  faCalendar: any = faCalendar;
  faChartLine: any = faChartLine;
  faPaw: any = faPaw;
  faUser: any = faUser;
  faEllipsis: any = faEllipsis;

  constructor(public nav: NavbarService, private _router: Router) { }

}
