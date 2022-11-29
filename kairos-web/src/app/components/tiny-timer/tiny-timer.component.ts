import { Component } from '@angular/core';
import { TimerService } from 'src/app/services/timer/timer.service';

@Component({
  selector: 'app-tiny-timer',
  templateUrl: './tiny-timer.component.html',
  styleUrls: ['./tiny-timer.component.scss']
})
export class TinyTimerComponent {
  constructor(public timer: TimerService) { }
}
