import { Injectable } from '@angular/core';
import { IAlertModel } from 'src/app/models/IAlertModel';

@Injectable({
  providedIn: 'root'
})
export class AlertDialogService {
  alertModel?: IAlertModel;
  isDisplayed: boolean;

  constructor() {
    this.isDisplayed = false;
  }

  displayAlert = (alertParams: IAlertModel) => {
    this.isDisplayed = true;
    this.alertModel = alertParams;

    setTimeout(() => this.hideAlert(), 3000)
  }

  hideAlert = () => {
    this.isDisplayed = false;
  }
}
