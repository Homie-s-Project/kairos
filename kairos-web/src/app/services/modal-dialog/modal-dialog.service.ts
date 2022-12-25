import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ModalDialogService {
  base: any;
  title?: string;
  isDisplayed: boolean;
  modalValue: EventEmitter<boolean> = new EventEmitter();

  constructor() { 
    this.isDisplayed = false;
  }

  displayModal = (title: string) => {
    this.title = title;
    this.isDisplayed = true;
  }

  hideModal = (confirmValue: boolean) => {
    this.isDisplayed = false;
    this.modalValue.emit(confirmValue);
  }
}
