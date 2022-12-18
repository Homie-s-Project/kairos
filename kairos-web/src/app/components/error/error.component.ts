import { Component, OnDestroy} from '@angular/core';
import { faLeftLong, faHeartCrack } from '@fortawesome/free-solid-svg-icons';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnDestroy{
  faLeftLong = faLeftLong;
  faHearthCrack = faHeartCrack;

  constructor(private nav: NavbarService) {
    nav.hideNavbar();
    
  }
  ngOnDestroy(): void {
    this.nav.showNavbar();
  }
}
