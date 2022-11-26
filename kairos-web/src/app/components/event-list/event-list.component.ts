import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IEventModel } from 'src/app/models/ievent.model';
import { CalendarService } from 'src/app/service/calendar.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss']
})
export class EventListComponent implements OnInit {

  @Input() events: IEventModel[] = [];
  @Output() onRemoveEventClicked: EventEmitter<IEventModel> = new EventEmitter<IEventModel>();

  constructor(private calendarService: CalendarService) { }

  ngOnInit(): void {
  }

}
