import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {IGroupModel} from "../../models/IGroupModel";
import {EventService} from "../../services/event/event.service";
import {ILabelModel} from "../../models/ILabelModel";
import {GroupService} from "../../services/group/group.service";
import {LabelService} from "../../services/label/label.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-event-calendar',
  templateUrl: './event-calendar.component.html',
  styleUrls: ['./event-calendar.component.scss']
})
export class EventCalendarComponent implements OnInit {

  groups?: IGroupModel[];
  labels?: ILabelModel[];

  eventForm: FormGroup<any> = new FormGroup<any>({
    group: new FormControl('', [Validators.required]),
    label: new FormControl('', [Validators.required]),
    title: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]),
    description: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(250)]),
    date: new FormControl('', [Validators.required]),
  });

  constructor(private eventService: EventService,
              private groupService: GroupService,
              private labelService: LabelService,
              private router: Router) {

    this.groupService.getGroups().subscribe(groups => {
      this.groups = groups;
    });

    this.labelService.getLabels().subscribe(labels => {
      this.labels = labels;
    });
  }

  ngOnInit(): void {
  }

  addEvent() {
    this.eventService.createEvent(this.eventForm.value).subscribe(() => {
      this.router.navigate(['/calendar']);
    });
  }
}
