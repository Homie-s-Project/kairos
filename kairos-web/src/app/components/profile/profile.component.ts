import {Component} from '@angular/core';
import {faPencil, faSquarePlus, faTrashCan, faCheck, faChevronDown} from '@fortawesome/free-solid-svg-icons'
import { map } from 'rxjs';
import { UserModel } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent{
  faPencil = faPencil;
  faTrashCan = faTrashCan;
  faSquarePlus = faSquarePlus;
  faCheck = faCheck;
  faChevronDown = faChevronDown;
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
    
  currentUser?: UserModel;

  constructor (private nav: NavbarService, private auth: AuthService) {
    nav.showBackButton();
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }
  
      this.currentUser = new UserModel(resp.body);
    })
  }
}
