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
    switch (this.alertModel?.alertType) {
      case "alert": {
        this.renderer.addClass(document.getElementById('alert-box'), 'kairos-alert');
        this.alertIcon = faTriangleExclamation;
        break;
      }
      case "valid": {
        this.renderer.addClass(document.getElementById('alert-box'), 'kairos-valid');
        this.alertIcon = faCircleCheck;
        break;
      }
      default : {
        this.renderer.addClass(document.getElementById('alert-box'), 'kairos-info');
        this.alertIcon = faCircleInfo;
        break;
      }
    }
  }
  ngOnDestroy(): void {
    switch (this.alertModel?.alertType) {
      case "alert": {
        this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-alert');
        break;
      }
      case "valid": {
        this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-valid');
        break;
      }
      default : {
        this.renderer.removeClass(document.getElementById('alert-box'), 'kairos-info');
        break;
      }
    }
  }
}
