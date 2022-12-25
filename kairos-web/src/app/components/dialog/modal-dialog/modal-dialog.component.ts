import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ModalDialogService } from 'src/app/services/modal-dialog/modal-dialog.service';

@Component({
  selector: 'app-modal-dialog',
  templateUrl: './modal-dialog.component.html',
  styleUrls: ['./modal-dialog.component.scss']
})
export class ModalDialogComponent {
  @Input() titleModal?: string;

  constructor(public modalDialog: ModalDialogService) { 
    if (this.titleModal == undefined) {
      this.titleModal = "Titre de modal";
    }
  }

  confirm = (value: boolean) => {
    this.modalDialog.hideModal(value);
  }
}
