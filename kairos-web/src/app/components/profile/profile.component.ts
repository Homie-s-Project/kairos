import {Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  title='chaman gautam';
  datasaved=false;
  groupForm!: FormGroup;
  GroupUpdate=null;

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

  constructor (private nav: NavbarService, private auth: AuthService, private formbuilder:FormBuilder, private groupservice:GroupService) {
    nav.showBackButton();
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }
  
      this.currentUser = new UserModel(resp.body);
    })
    this.GetGroups();
  }
  ngOnInit(){
    this.groupForm=this.formbuilder.group({
      title:[' ',[Validators.required]]
  });}    

  IsEditable()
  {
    this.visible = !this.visible;
  }

  onFormSubmit(){
    this.datasaved=false;
    let group=this.groupForm.value;
    this.CreateGroups(group);
    this.groupForm.reset();
  }
  CreateGroups(group:IGroupModel){
    if(this.GroupUpdate==null){
  
    this.groupservice.CreateGroups(group).subscribe(group=>{
      this.datasaved=true;
      this.GetGroups();
  
    });
  }
}
  

  GetGroups(){
    this.allgroups=this.groupservice.GetGroups();
    }
    

  GroupDelete(groupid:number)
  {
    this.groupservice.GroupDelete(groupid)
   .subscribe(group=>{
     this.GetGroups();
   })

  }
}
