import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {faCheck, faChevronDown, faPencil, faSquarePlus, faTimes, faTrashCan} from '@fortawesome/free-solid-svg-icons'
import {Observable} from 'rxjs';
import {IGroupModel} from 'src/app/models/IGroupModel';
import {ILabelModel} from 'src/app/models/ILabelModel';
import {UserModel} from 'src/app/models/user.model';
import {AuthService} from 'src/app/services/auth/auth.service';
import {GroupService} from 'src/app/services/group/group.service';
import {LabelService} from 'src/app/services/label/label.service';
import {NavbarService} from 'src/app/services/navbar/navbar.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  faPencil = faPencil;
  faTrashCan = faTrashCan;
  faSquarePlus = faSquarePlus;
  faCheck = faCheck;
  faChevronDown = faChevronDown;
  faTimes = faTimes;

  /*Variable de groupe*/
  allgroups: Observable<IGroupModel[]> | undefined;
  groupDatasaved = false;
  groupForm!: FormGroup;
  groupUpdate = null;
  groupVisible: boolean = false

  /*Variable de label*/
  allLabels?: ILabelModel[];
  labelDatasaved = false;
  labelForm!: FormGroup;
  labelUpdate?: boolean;
  labelIdUpdate?: number;

  labelVisible: boolean = false
  createLabel: boolean = false

  currentUser?: UserModel;

  constructor(
    private nav: NavbarService,
    private auth: AuthService,
    private router: Router,
    private formbuilder: FormBuilder,
    private groupservice: GroupService,
    private labelservice: LabelService) {
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

  ngOnInit() {
    this.groupForm = this.formbuilder.group({
      title: [' ', [Validators.required]]
    });
    this.labelForm = this.formbuilder.group({
      name: [' ', [Validators.required]]
    });
  }

  isLabelEditable(labelId: number) {
    this.labelVisible = !this.labelVisible;
    this.labelIdUpdate = labelId;
  }

  updateLabel(labelId: number) {
    // @ts-ignore
    let labelName = document.getElementById('edit-input-' + labelId)?.value;

    if (labelName != "") {
      this.labelservice.updateLabel(labelId, labelName).subscribe(resp => {
        this.getLabels();

        this.labelUpdate = false;
        this.labelIdUpdate = undefined;
        this.labelVisible = false;
      })
    }
  }

  onLabelFormSubmit() {
    this.labelDatasaved = false;
    let labelName = this.labelForm.get("name")?.value;

    if (labelName) {
      this.createLabels(labelName);
    }
  }

  newLabel() {
    this.createLabel = true;
    this.labelForm.patchValue({name: ""});
  }

  createLabels(label: string) {
    if (!this.labelUpdate) {
      this.labelservice.createLabels(label).subscribe(label => {
        this.labelDatasaved = true;
        this.getLabels();
        this.labelForm.reset();
      });
    }
  }

  cancelCreation() {
    this.createLabel = false;
  }


  getLabels() {
    this.labelservice.getLabels().subscribe(resp => {
      console.log(resp);
      this.allLabels = resp;
    });
  }


  labelDelete(labelid: number) {
    this.labelservice.labelDelete(labelid).subscribe(resp => {
      console.log(resp);
      this.updateLabels(resp.labelId);
    })
  }

  updateLabels(deleteId: number) {
    var subLabels = this.allLabels;
    this.allLabels = [];

    subLabels?.forEach(element => {
      if (element.labelId != deleteId) {
        this.allLabels?.push(element)
      }
    });
  }

  /*Méthode des groupes*/
  isGroupEditable() {
    this.groupVisible = !this.groupVisible;
  }

  onGroupFormSubmit() {
    this.groupDatasaved = false;
    let group = this.groupForm.value;
    this.CreateGroups(group);
    this.groupForm.reset();
  }

  CreateGroups(group: IGroupModel) {
    if (this.groupUpdate == null) {

      this.groupservice.createGroups(group).subscribe(group => {
        this.groupDatasaved = true;
        this.getGroups();

      });
    }
  }

  getGroups() {
    this.allgroups = this.groupservice.getGroups();
  }

  GroupDelete(groupid: number) {
    this.groupservice.groupDelete(groupid)
      .subscribe(group => {
        this.getGroups();
      })

  }
}
