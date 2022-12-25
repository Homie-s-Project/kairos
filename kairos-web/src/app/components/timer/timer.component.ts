import { animate, AUTO_STYLE, state, style, transition, trigger } from '@angular/animations';
import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faCircleChevronUp, faCircleChevronDown, faCircleChevronLeft, faCircleChevronRight, faEllipsis } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { ILabelModel } from 'src/app/models/ILabelModel';
import { AlertDialogService } from 'src/app/services/alert-dialog/alert-dialog.service';
import { LabelService } from 'src/app/services/label/label.service';
import { NavbarService } from 'src/app/services/navbar/navbar.service';
import { TimerService } from 'src/app/services/timer/timer.service';

const collapseAnimation = trigger('collapse', [
  state('false', style({ height: AUTO_STYLE, visibility: AUTO_STYLE, })),
  state('true', style({ height: '0', visibility: 'hidden' })),
  transition('false => true', animate('500ms ease-out')),
  transition('true => false', animate('500ms ease-out'))
]);

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.scss'],
  animations: [
    collapseAnimation
  ]
})
export class TimerComponent implements OnInit, OnDestroy {
  private subscription?: Subscription = new Subscription();
  labels?: ILabelModel[];

  labelForm: FormGroup<any> = new FormGroup<any>({
    label: new FormControl('', [Validators.required])
  })

  animeState: string = "";

  faCircleChevronUp = faCircleChevronUp;
  faCircleChevronDown = faCircleChevronDown;
  faCircleChevronLeft = faCircleChevronLeft;
  faCircleChevronRight = faCircleChevronRight;
  faEllipsis = faEllipsis;

  constructor(public nav: NavbarService, 
    public timer: TimerService,
    public alertDialog: AlertDialogService,
    private label: LabelService, 
    private renderer: Renderer2) {
    this.renderer.addClass(document.getElementById('app-container'), 'kairos-timer');
    this.label.getLabels().subscribe(resp => {
      this.labels = resp;
    })
    
    this.subscription = this.labelForm.get('label')?.valueChanges
      .subscribe(l => {
        this.onLabelChange(l)
      }
    );
  }

  ngOnInit(): void {
    this.nav.hideBackButton();
    this.timer.isTinyVisible = false;
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.getElementById('app-container'), 'kairos-timer');
    this.timer.isTinyVisible = true;
    this.subscription.forEach((sub) => sub.unsubscribe());
  }

  onLabelChange(l: any) {
    this.timer.labelId = l.toString();
  }

  startTimer = () => {
    // Get circle countdown element
    const circle = document.getElementById('circle-countdown');

    // Play Animation (commenté mais à garder bug encore présent)
    // if (circle != null) {
    //   circle.classList.add('circle-anim-countdown');
    //   circle.style.animationPlayState = "running";
    //   circle.style.animationDuration = `${(this.timer.minute * 60) + this.timer.second}s`;
    // }

    // Start countdown
    try {
      this.timer.startCountdown();
    } catch (error) {
      this.alertDialog.displayAlert({alertMessage: (error as DOMException).message, alertType: 'info'})
      return;
    }

    this.timer.isStarted = true;
    
    // Collapse animation on timer container
    setTimeout(() => {
      const container = document.getElementById('timer-countdown-container');
      container?.classList.add('hidden')

      this.timer.isCollapsed = true
    }, 2000)
  }

  stop = () => {
    // Get circle countdown element
    const circle = document.getElementById('circle-countdown');

    // Call stop timer
    if (this.timer.openModalCancelTimer()) {
      this.timer.isCollapsed = false;
      this.timer.isStarted = false;
  
      // Stop Animation (commenté mais à garder bug encore présent)
      // if (circle != null) {
      //   circle.classList.remove('circle-anim-countdown');
      //   circle.style.animationPlayState = "paused";
      // }
    }
  }

  expend = () => {
    if (this.timer.isCollapsed == true) {
      this.timer.isCollapsed = false;

      setTimeout(() => {
        const container = document.getElementById('timer-countdown-container');
        container?.classList.remove('hidden')
      }, 500)
    } else {
      this.timer.isCollapsed = true;
  
      const container = document.getElementById('timer-countdown-container');
      container?.classList.add('hidden')
    }
  }
}
