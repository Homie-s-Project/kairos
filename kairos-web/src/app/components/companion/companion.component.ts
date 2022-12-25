import { Component } from '@angular/core';
import { faPersonDigging } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-companion',
  templateUrl: './companion.component.html',
  styleUrls: ['./companion.component.scss']
})
export class CompanionComponent {
  faPersonDigging = faPersonDigging;
}
