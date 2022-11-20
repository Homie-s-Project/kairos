import {Component, Input, OnInit, Renderer2} from '@angular/core';
import { faPencil, faTrashCan } from '@fortawesome/free-solid-svg-icons'
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  faPencil : any = faPencil;
  faTrashCan : any = faTrashCan;
  labels = [
    {name : 'Science / Math'},
    {name : 'Economie'},
    {name : 'Allemand'},
    {name : 'Anglais'},
    {name : 'Informatique'},
  ]
  constructor(private renderer: Renderer2) {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
  }

}
