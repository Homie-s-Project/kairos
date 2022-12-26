import { Component } from '@angular/core';
import { faPersonDigging } from '@fortawesome/free-solid-svg-icons';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-companion',
  templateUrl: './companion.component.html',
  styleUrls: ['./companion.component.scss']
})
export class CompanionComponent {
  faPersonDigging = faPersonDigging;

  constructor(private nav: NavbarService) {
    nav.showBackButton();
  }
}
