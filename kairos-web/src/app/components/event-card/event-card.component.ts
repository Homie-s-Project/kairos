import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { CalendarService, EventModel } from 'src/app/service/calendar.service';
import { faEllipsisVertical } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-event-card',
  templateUrl: './event-card.component.html',
  styleUrls: ['./event-card.component.scss']
})
export class EventCardComponent implements OnInit, OnDestroy {
  faEllipsisVertical: any = faEllipsisVertical;

  @Input() event: EventModel|undefined;
  @Output() onRemoveClicked: EventEmitter<EventModel> = new EventEmitter<EventModel>();

  constructor(
    private calendarService: CalendarService
  ) {
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

}
