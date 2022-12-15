import {Component, OnInit, Renderer2} from '@angular/core';
import {faPencil, faSquarePlus, faTrashCan, faCheck} from '@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  faPencil = faPencil;
  faTrashCan = faTrashCan;
  faSquarePlus = faSquarePlus;
  faCheck = faCheck;
  labels = [
    {
      id : 1,
      name : 'Science / Math'
    },
    {
      id : 2,
      name : 'Economie'
    },
  ]
  groups = [
    { 
      id : 1,
      name : 'Groupe name',
      members  : [
        'Chris',
        'William',
        'Romain'
      ]
    },
    { 
      id : 2,
      name : 'Groupe kaka',
      members  : [
        'Chris',
        'William',
        'Romain'
      ]
    }
  ]
  visible:boolean = false

  isEditable()
  {
    this.visible = !this.visible;
  }

  constructor(private renderer: Renderer2) {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
    
  }

}
