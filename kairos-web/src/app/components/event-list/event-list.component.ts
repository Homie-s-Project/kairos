import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CalendarService, EventModel } from 'src/app/service/calendar.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss']
})
export class EventListComponent implements OnInit {

  @Input() events: EventModel[] = [];
  @Output() onRemoveEventClicked: EventEmitter<EventModel> = new EventEmitter<EventModel>();

  constructor(
    private calendarService: CalendarService
  ) { }

  ngOnInit(): void {
  }

}
