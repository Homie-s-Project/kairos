import { Component } from '@angular/core';
import { AlertDialogService } from './services/alert-dialog/alert-dialog.service';
import { ModalDialogService } from './services/modal-dialog/modal-dialog.service';
import { TimerService } from './services/timer/timer.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent{
  constructor(public timer: TimerService, 
    public modalDialog: ModalDialogService, 
    public alertDialog: AlertDialogService) { }
}
