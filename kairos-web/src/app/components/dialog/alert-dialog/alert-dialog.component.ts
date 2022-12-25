import { Component, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { faCircleInfo, faCircleCheck, faTriangleExclamation, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { IAlertModel } from 'src/app/models/IAlertModel';

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.scss']
})
export class AlertDialogComponent implements OnInit, OnDestroy {
  @Input() alertModel?: IAlertModel;
  alertIcon: IconDefinition;

  constructor (private renderer: Renderer2) {
    this.alertIcon = faCircleInfo;
  }

  ngOnInit(): void {
    if (this.alertModel?.alertType === "alert") {
      this.renderer.addClass(document.getElementById('alert-box'), 'kairos-alert');
      this.alertIcon = faTriangleExclamation;
    } else if (this.alertModel?.alertType === "valid") {
      this.renderer.addClass(document.getElementById('alert-box'), 'kairos-valid');
      this.alertIcon = faCircleCheck;
    } else {
      this.renderer.addClass(document.getElementById('alert-box'), 'kairos-info');
      this.alertIcon = faCircleInfo;
    }
  }
  ngOnDestroy(): void {
    if (this.alertModel?.alertType === "alert") {
      this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-alert');
    } else if (this.alertModel?.alertType === "valid") {
      this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-valid');
    } else {
      this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-info');
    }
  }
}
