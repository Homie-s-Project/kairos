import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CalendarService } from 'src/app/services/calendar/calendar.service';
import { faTag, faCalendar } from '@fortawesome/free-solid-svg-icons';
import { IEventModel } from 'src/app/models/ievent.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.scss']
})
export class EventCardComponent implements OnInit, OnDestroy {
  faTag = faTag;
  faCalendar = faCalendar;

  @Input() event: IEventModel|undefined;
  @Output() onRemoveClicked: EventEmitter<IEventModel> = new EventEmitter<IEventModel>();

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
