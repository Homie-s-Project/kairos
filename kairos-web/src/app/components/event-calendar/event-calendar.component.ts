import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {IGroupModel} from "../../models/IGroupModel";
import {IEventForm} from "../../services/event/event.service";
import {ILabelModel} from "../../models/ILabelModel";

@Component({
  selector: 'app-event-calendar',
  templateUrl: './event-calendar.component.html',
  styleUrls: ['./event-calendar.component.scss']
})
export class EventCalendarComponent {

  groups?: IGroupModel[];
  labels?: ILabelModel[];

  eventForm: FormGroup<any> = new FormGroup<any>({
    group: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]),
    label: new FormControl('', [Validators.required]),
    title: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(250)]),
    description: new FormControl('', [Validators.required]),
    date: new FormControl('', [Validators.required]),
  });

  constructor() {
    this.groups = [
      {
        groupId: 1,
        groupName: "Group #1",
        groupIsPrivate: true,
      }
      , {
        groupId: 2,
        groupName: "Group #2",
        groupIsPrivate: false,
      },
      {
        groupId: 3,
        groupName: "Group #3",
        groupIsPrivate: false,
      }
    ];

    this.labels = [
      {
        labelId: 1,
        labelTitle: "Label #1",
      },
      {
        labelId: 2,
        labelTitle: "Label #2",
      }
    ];
  }

  addEvent() {
    console.log(this.eventForm.value);
  }
}
