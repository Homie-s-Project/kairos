import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EventModel } from 'src/app/models/event.model';
import { CalendarService } from 'src/app/services/calendar/calendar.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss']
})
export class EventListComponent implements OnInit {

  @Input() events: EventModel[] = [];
  @Output() onRemoveEventClicked: EventEmitter<EventModel> = new EventEmitter<EventModel>();

  constructor(private calendarService: CalendarService) { }

  ngOnInit(): void {
  }

}
