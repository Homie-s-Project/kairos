import { Component, OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { faCalendar, faChartLine, faPaw,faUser } from '@fortawesome/free-solid-svg-icons'
import { NavbarService } from 'src/app/service/navbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faCalendar: any = faCalendar;
  faChartLine: any = faChartLine;
  faPaw: any = faPaw;
  faUser: any = faUser;

  constructor(public nav: NavbarService, private _router: Router, private renderer: Renderer2) { }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  goToTimer = () => {
    this.renderer.removeClass(document.body, 'landing-background')
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
    this._router.navigate(['timer'])
  }

  goToCalendar = () => {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
    this._router.navigate(['calendar'])
  }

  goToStatistics = () => {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
    this._router.navigate(['statistics'])
  }

  goToCompanion = () => {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
    throw new Error('Method not implemented.');
  }

  goToProfil = () => {
    throw new Error('Method not implemented.');
  }

}
