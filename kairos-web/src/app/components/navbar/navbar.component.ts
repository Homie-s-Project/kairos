import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faCircleLeft, faCalendar, faChartLine, faPaw, faUser, faEllipsis } from '@fortawesome/free-solid-svg-icons'
import { NavbarService } from 'src/app/service/navbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faCircleLeft: any = faCircleLeft;
  faCalendar: any = faCalendar;
  faChartLine: any = faChartLine;
  faPaw: any = faPaw;
  faUser: any = faUser;
  faEllipsis: any = faEllipsis;

  constructor(public nav: NavbarService, private _router: Router) { }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  goToTimer = () => {
    this.nav.hideBackButton();
    console.log(this.nav.isBackVisible)
    this._router.navigate(['timer']);
  }

  goToCalendar = () => {
    this.nav.showBackButton();
    console.log(this.nav.isBackVisible)
    this._router.navigate(['calendar']);
  }

  goToStatistics = () => {
    this.nav.showBackButton();
    console.log(this.nav.isBackVisible)
    this._router.navigate(['statistics']);
  }

  goToCompanion = () => {
    throw new Error('Method not implemented.');
  }

  goToProfil = () => {
    throw new Error('Method not implemented.');
  }

}
