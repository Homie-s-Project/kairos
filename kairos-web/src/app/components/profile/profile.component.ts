import {Component} from '@angular/core';
import {faPencil, faSquarePlus, faTrashCan, faCheck, faChevronDown} from '@fortawesome/free-solid-svg-icons'
import { map, Observable } from 'rxjs';
import { IGroupModel } from 'src/app/models/IGroupModel';
import { UserModel } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { GroupService } from 'src/app/services/group/group.service';
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
  allgroups:Observable<IGroupModel[]> | undefined;
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

  currentUser?: UserModel;

  constructor (private nav: NavbarService, private auth: AuthService, private groupservice:GroupService) {
    nav.showBackButton();
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }
  
      this.currentUser = new UserModel(resp.body);
    })
    this.getGroups();
  }

  IsEditable()
  {
    this.visible = !this.visible;
  }

  getGroups(){
    this.allgroups=this.groupservice.getGroups();
     }
    

  GroupDelete(groupid:number)
  {
    this.groupservice.GroupDelete(groupid)
   .subscribe(group=>{
     this.getGroups();
   })

  }
}
