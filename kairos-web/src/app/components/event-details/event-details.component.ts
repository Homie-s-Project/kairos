import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Subscription } from 'rxjs';
import { CalendarService, EventModel } from 'src/app/service/calendar.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit {

  public event?: EventModel;

  private subscriptions: Subscription[] = [];

  constructor(
      private route: ActivatedRoute,
      private calendarService: CalendarService
  ) { }

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
