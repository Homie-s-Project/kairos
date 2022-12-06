import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { faPencil } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { IEventModel } from 'src/app/models/ievent.model';
import { CalendarService } from 'src/app/services/calendar/calendar.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})

export class EventDetailsComponent implements OnInit, OnDestroy {
  faPencil = faPencil;
  event?: IEventModel;
  subscriptions: Subscription[] = [];
  isEditing: boolean = false;

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