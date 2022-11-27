import { animate, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { faCircleChevronUp, faCircleChevronDown, faCircleChevronLeft, faCircleChevronRight, faEllipsis } from '@fortawesome/free-solid-svg-icons';
import { NavbarService } from 'src/app/service/navbar.service';

const leavingTransition = transition(':enter', [
  style({transform: 'translateY(0%)'}),
  animate('1000ms ease-out', style({transform: 'translateY(155%)'}))
]);

const slideOutDownAnimation = trigger('slideOutDown', 
  [leavingTransition]);

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.scss'],
  animations: [slideOutDownAnimation]
})
export class TimerComponent implements OnInit {
  minute: number = 0;
  second: number = 0;
  minString: string = "";
  secString: string = "";
  animeState: string = "";

  base: any;
  faCircleChevronUp = faCircleChevronUp;
  faCircleChevronDown = faCircleChevronDown;
  faCircleChevronLeft = faCircleChevronLeft;
  faCircleChevronRight = faCircleChevronRight;

  constructor(public nav: NavbarService, private renderer: Renderer2) {
    this.renderer.removeClass(document.body, 'landing-background')
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
    this.minString = this.updateTime(this.minute);
    this.secString = this.updateTime(this.second);
    this.animeState = 'paused';
  }

  ngOnInit(): void {
    this.nav.hideBackButton();
    this.minString = this.updateTime(this.minute);
    this.secString = this.updateTime(this.second);
    this.animeState = 'paused';
  }

  startTimer = () => {
    // Get circle countdown element
    const circle = document.getElementById('circle-countdown');

    if (this.minute == 0 && this.second == 0) {
      return;
    }
    
    this.nav.showNavbarTongue()

    // Play Animation
    if (circle != null) {
      circle.classList.add('circle-anim-countdown');
      circle.style.animationPlayState = "running";
      circle.style.animationDuration = `${(this.minute * 60) + this.second}s`;
    }

    // Start countdown
    this.base = setInterval(this.timer, 1000);
  }

  timer = () => {
    this.second--;

    if (this.second < 0) {
      this.minute--;

      if (this.minute <= 0 && this.second <= 0) {
        this.stop();
        return;
      }

      this.second = 59;
    }

    this.minString = this.updateTime(this.minute);
    this.secString = this.updateTime(this.second);
  }

  stop = () => {
    // Get circle countdown element
    const circle = document.getElementById('circle-countdown');

    if (circle != null) {
      circle.classList.remove('circle-anim-countdown');
      circle.style.animationPlayState = "paused";
    }

    clearInterval(this.base);
    this.minute = 0;
    this.second = 0;
    this.minString = this.updateTime(this.minute);
    this.secString = this.updateTime(this.second);

    this.nav.hideNavbarTongue()
  }

  updateTime = (i: number) => {
    return i < 10 ? `0${i}` : `${i}`;
  }

  upTime = () => {
    if (this.minute < 100) {
      this.minute += 5;
      this.minString = this.updateTime(this.minute);
    }
  }

  downTime = () => {
    if (this.minute > 0) {
      this.minute -= 5;
      this.minString = this.updateTime(this.minute);
    }
  }
}
