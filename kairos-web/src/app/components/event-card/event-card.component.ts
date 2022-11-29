import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { faTag, faCalendar } from '@fortawesome/free-solid-svg-icons';
import { EventModel } from 'src/app/models/event.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.scss']
})
export class EventCardComponent implements OnInit, OnDestroy {
  faTag = faTag;
  faCalendar = faCalendar;

  @Input() event: EventModel|undefined;
  @Output() onRemoveClicked: EventEmitter<EventModel> = new EventEmitter<EventModel>();

  constructor(private calendarService: CalendarService, private _router: Router, ) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

  goToEvent = (eventId: number|undefined) => {
    if (typeof eventId == undefined) {
      alert('L\'event ne peut pas être "undefined"')
      return 
    }

    this._router.navigate(['/calendar', eventId])
  }
}
