import {Component, OnInit, Renderer2} from '@angular/core';
import {faPencil, faSquarePlus, faTrashCan} from '@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  faPencil = faPencil;
  faTrashCan = faTrashCan;
  faSquarePlus = faSquarePlus  ;
  labels = [
    {name : 'Science / Math'},
    {name : 'Economie'},
    {name : 'Allemand'},
    {name : 'Anglais'},
    {name : 'Informatique'},
  ]
  groups = [
    {name : 'Skt t1'},
    {name : 'Los pollos hermanos'},
  ]
  members  = [
    {name : 'Chris'},
    {name : 'William'},
    {name : 'Romain'},
    {name : 'Alexandre'},
    {name : 'Clyve'},
  ]
  constructor(private renderer: Renderer2) {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
  }

}
