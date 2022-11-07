import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {

  constructor(private renderer: Renderer2) { 
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
  }

}
