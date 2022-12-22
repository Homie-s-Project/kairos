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
  GroupUpdate=null;
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
    this.GetLabels();
    this.GetGroups();
  }
  ngOnInit(){
    this.groupForm=this.formbuilder.group({
      title:[' ',[Validators.required]]
    });
    this.labelForm=this.formbuilder.group({
      name:[' ',[Validators.required]]
    });
  }    

  IsLabelEditable()
  {
    this.labelVisible = !this.labelVisible;
  }

  updateLabel(labelid:number)
  {
    this.labelVisible = !this.labelVisible;
    this.labelservice.updateLabel(labelid).subscribe(resp => {
      console.log(resp);
      this.GetLabels();
    })
  }

  onLabelFormSubmit(){
    this.labelDatasaved=false;
    let label=this.labelForm.value;
    this.CreateLabels(label);
    this.labelForm.reset();
  }
  newLabel()
  {
    this.createLabel = true;
    var inputLabel = document.getElementById('newLabel');
  }
  CreateLabels(label:ILabelModel){
    if(this.LabelUpdate==null){
  
    this.labelservice.CreateLabels(label).subscribe(label=>{
      this.labelDatasaved=true;
      this.GetLabels();
      
    });
  }
}
cancelCreation()
{
  this.createLabel = false;
}
  

  GetLabels(){
    this.labelservice.GetLabels().subscribe(resp => {
      console.log(resp);
      this.allLabels = resp;
    });
    }
    

  LabelDelete(labelid:number)
  {
    this.labelservice.LabelDelete(labelid).subscribe(resp => {
      console.log(resp);
      this.updateLabels(resp.labelId);
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
