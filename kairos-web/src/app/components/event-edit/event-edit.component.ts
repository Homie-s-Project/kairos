import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Subscription } from 'rxjs';
import { IEventModel } from 'src/app/models/ievent.model';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.scss']
})
export class EventEditComponent implements OnInit {
  event?: IEventModel;
  subscriptions: Subscription[] = [];
  isEditing: boolean = false;

  public eventForm: FormGroup<any> = new FormGroup<any> ({
    eventTitle: new FormControl('', [Validators.required]),
    eventDescription: new FormControl('', [Validators.required]),
    eventDueDate: new FormControl('', [Validators.required]),
    eventLabel: new FormControl()
  })

  constructor(private route: ActivatedRoute, private calendarService: CalendarService) { }

  ngOnInit(): void {
    this.subscriptions.push(
        this.route.paramMap.subscribe((map: ParamMap) => {
          const eventId: number = parseInt(map.get('eventId') || '');
          this.event = this.calendarService.getEvent(eventId);
        })
    );
  }

  ngOnDestroy(): void {
    for (let subscription of this.subscriptions) {
      subscription.unsubscribe();
    }
  }
}
