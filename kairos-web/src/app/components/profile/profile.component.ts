import {Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {faPencil, faSquarePlus, faTrashCan, faCheck, faChevronDown, faTimes} from '@fortawesome/free-solid-svg-icons'
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
  faTimes = faTimes;
  /*Variable de groupe*/
  allgroups:Observable<IGroupModel[]> | undefined;
  groupDatasaved=false;
  groupForm!: FormGroup;
  groupUpdate=null;
  groupVisible:boolean = false

  /*Variable de label*/
  allLabels?:ILabelModel[];
  labelDatasaved=false;
  labelForm!: FormGroup;
  LabelUpdate=null;
  labelVisible:boolean = false
  createLabel:boolean = false

  currentUser?: UserModel;

  constructor (
    private nav: NavbarService, 
    private auth: AuthService,
    private router: Router,
    private formbuilder:FormBuilder, 
    private groupservice:GroupService, 
    private labelservice:LabelService) {
    nav.showBackButton();
    auth.getProfile().subscribe(resp => {
      if (resp.status != 200) {
        console.log(resp.statusText);
      }
  
      this.currentUser = new UserModel(resp.body);
    })
    this.getLabels();
    this.getGroups();
  }
  ngOnInit(){
    this.groupForm=this.formbuilder.group({
      title:[' ',[Validators.required]]
    });
    this.labelForm=this.formbuilder.group({
      name:[' ',[Validators.required]]
    });
  }    

  isLabelEditable()
  {
    this.labelVisible = !this.labelVisible;
  }

  updateLabel(labelid:number)
  {
    this.labelVisible = !this.labelVisible;
    this.labelservice.updateLabel(labelid).subscribe(resp => {
      console.log(resp);
      this.getLabels();
    })
  }

  onLabelFormSubmit(){
    this.labelDatasaved=false;
    let label=this.labelForm.value;
    this.createLabels(label);
    this.labelForm.reset();
  }
  newLabel()
  {
    this.createLabel = true;
    var inputLabel = document.getElementById('newLabel');
  }
  createLabels(label:ILabelModel){
    if(this.LabelUpdate==null){
  
    this.labelservice.createLabels(label).subscribe(label=>{
      this.labelDatasaved=true;
      this.getLabels();
      
    });
  }
}
cancelCreation()
{
  this.createLabel = false;
}
  

  getLabels(){
    this.labelservice.getLabels().subscribe(resp => {
      console.log(resp);
      this.allLabels = resp;
    });
    }
    

  labelDelete(labelid:number)
  {
    this.labelservice.labelDelete(labelid).subscribe(resp => {
      console.log(resp);
      this.updateLabels(resp.labelId);
    })
  }

  /*Méthode des groupes*/

  isGroupEditable()
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
    if(this.groupUpdate==null){
  
    this.groupservice.createGroups(group).subscribe(group=>{
      this.groupDatasaved=true;
      this.getGroups();
  
    });
    }
  }
  
  updateLabels(deleteId: number) {
    var subLabels = this.allLabels;
    this.allLabels = [];

    subLabels?.forEach(element => {
      if (element.labelId != deleteId)
      {
        this.allLabels?.push(element)
      }
    });
  }

  getGroups(){
    this.allgroups=this.groupservice.getGroups();
    }
    

  GroupDelete(groupid:number)
  {
    this.groupservice.groupDelete(groupid)
   .subscribe(group=>{
     this.getGroups();
   })

  }
}
