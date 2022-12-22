import {Component} from '@angular/core';
import {faPencil, faSquarePlus, faTrashCan} from '@fortawesome/free-solid-svg-icons'
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
