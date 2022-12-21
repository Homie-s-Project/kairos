import {Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {faPencil, faSquarePlus, faTrashCan, faCheck, faChevronDown} from '@fortawesome/free-solid-svg-icons'
import { map, Observable } from 'rxjs';
import { IGroupModel } from 'src/app/models/IGroupModel';
import { ILabelModel } from 'src/app/models/ILabelModel';
import { UserModel } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { GroupService } from 'src/app/services/group/group.service';
import { LabelService } from 'src/app/services/label/label.service';
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
  /*Variable de label*/
  allgroups:Observable<IGroupModel[]> | undefined;
  title='';
  groupDatasaved=false;
  groupForm!: FormGroup;
  GroupUpdate=null;
  /*Variable de groupe*/
  allLabels:Observable<ILabelModel[]> | undefined;
  name='';
  labelDatasaved=false;
  labelForm!: FormGroup;
  LabelUpdate=null;

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
  groupVisible:boolean = false
  labelVisible:boolean = false

  currentUser?: UserModel;

  constructor (private nav: NavbarService, private auth: AuthService, private formbuilder:FormBuilder, private groupservice:GroupService, private labelservice:LabelService) {
    nav.showBackButton();
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }
  
      this.currentUser = new UserModel(resp.body);
    })
    this.GetLabels();
    this.GetGroups();
  }
  ngOnInit(){
    this.groupForm=this.formbuilder.group({
      title:[' ',[Validators.required]]
  });
    this.labelForm=this.formbuilder.group({
      name:[' ',[Validators.required]]
  });}    

  IsLabelEditable()
  {
    this.labelVisible = !this.labelVisible;
  }

  SaveLabel()
  {
    this.labelVisible = !this.labelVisible;
  }

  onLabelFormSubmit(){
    this.labelDatasaved=false;
    let label=this.labelForm.value;
    this.CreateLabels(label);
    this.labelForm.reset();
  }
  CreateLabels(label:ILabelModel){
    if(this.LabelUpdate==null){
  
    this.labelservice.CreateLabels(label).subscribe(label=>{
      this.labelDatasaved=true;
      this.GetLabels();
  
    });
  }
}
  

  GetLabels(){
    this.allLabels=this.labelservice.GetLabels();
    }
    

  LabelDelete(labelid:number)
  {
    this.labelservice.LabelDelete(labelid)
   .subscribe(label=>{
     this.GetGroups();
   })

  }

  /*Méthode des groupes*/

  IsGroupEditable()
  {
    this.groupVisible = !this.groupVisible;
  }

  onGroupFormSubmit(){
    this.groupDatasaved=false;
    let group=this.groupForm.value;
    this.CreateGroups(group);
    this.groupForm.reset();
  }
  CreateGroups(group:IGroupModel){
    if(this.GroupUpdate==null){
  
    this.groupservice.CreateGroups(group).subscribe(group=>{
      this.groupDatasaved=true;
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
