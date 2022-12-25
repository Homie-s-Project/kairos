import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Subscription } from 'rxjs';
import { AlertDialogService } from '../alert-dialog/alert-dialog.service';
import { ModalDialogService } from '../modal-dialog/modal-dialog.service';

@Injectable({
  providedIn: 'root'
})
export class TimerService {
  base: any;
  intervalHeartbeat: any;
  private subscription: Subscription = new Subscription();
  labelId: string = "";
  minuteStr: string;
  secondStr: string;
  minute: number = 0;
  second: number = 0;
  isTinyVisible: boolean = true;
  isStarted: boolean = false;
  isCollapsed: boolean = false;

  constructor(private modalDialog: ModalDialogService, 
    private alertDialog: AlertDialogService, 
    private http: HttpClient, 
    private auth: AuthService) {
    this.getValues();
    this.minuteStr = this.updateTime(this.minute);
    this.secondStr = this.updateTime(this.second);

    if (this.minuteStr != "00" && this.secondStr != "00") {
      this.startCountdown();
    } else {
      this.isTinyVisible = false;
    }
  }

  // Timer logic
  upTime = () => {
    if (this.minute < 100) {
      this.minute += 5;
      this.minuteStr = this.updateTime(this.minute);
    }
  }

  downTime = () => {
    if (this.minute > 0) {
      this.minute -= 5;
      this.minuteStr = this.updateTime(this.minute);
    }
  }

  updateTime = (i: number) => {
    return i < 10 ? `0${i}` : `${i}`;
  }

  startCountdown = () => {
    if (this.minute == 0 && this.second == 0) {
      throw new Error("Valeur du temps à 0, Veuillez en saisir une");
    }

    this.postStartStudies((this.minute + this.second).toString(), this.labelId).subscribe({
      error: (error: HttpErrorResponse) => {
        throw new Error(error.error.message);
      },
      next: () => {
        this.base = setInterval(this.timerCountdown, 1000);
        this.intervalHeartbeat = setInterval(this.heartbeatCall, 60000)
      }
    })

  }

  timerCountdown = () => {
    this.second--;

    if (this.second < 0) {
      this.minute--;

      if (this.minute < 0 && this.second <= 0) {
        this.stopCountdown();
        this.alertDialog.displayAlert({alertMessage: 'Studies terminée ! Bon travail !', alertType: 'valid'});
        return;
      }

      this.second = 59;
    }

    this.saveValues();
    this.minuteStr = this.updateTime(this.minute);
    this.secondStr = this.updateTime(this.second);
  }

  heartbeatCall = () => {
    const sub = this.postHeartbeatStudies().subscribe({
      error: (error: HttpErrorResponse) => {
        this.subscription.unsubscribe();
        this.alertDialog.displayAlert({alertMessage: error.error.message, alertType: 'alert'})
        this.stopCountdown();
      }
    })
  }

  openModalCancelTimer = ():boolean => {
    var result = false;
    this.modalDialog.displayModal('Voulez-vous vous annuler votre Studies ?')
    this.subscription = this.modalDialog.modalValue.subscribe((data => {
        if (data) {
          this.stopCountdown();
          result = data; 
        } else {
          this.subscription.unsubscribe();
        }
      })
    );
  
    return result;
  }

  stopCountdown = () => {
    const sub = this.postStopStudies().subscribe({
      error: (error: HttpErrorResponse) => {
        sub.unsubscribe();
      },
      next: () => {
        clearInterval(this.base);
        clearInterval(this.intervalHeartbeat);
        this.resetValues();
      }
    })
  }

  saveValues = () => {
    localStorage.setItem('minute', this.minute.toString());
    localStorage.setItem('second', this.second.toString());
    localStorage.setItem('isStarted', this.convertBooleanToStr(this.isStarted));
    localStorage.setItem('isCollapsed', this.convertBooleanToStr(this.isCollapsed));
  }

  getValues = () => {
    this.minute = this.convertLocStorageToNumber('minute');
    this.second = this.convertLocStorageToNumber('second');
    this.isStarted = this.convertLocStorageToBoolean('isStarted');
    this.isCollapsed = this.convertLocStorageToBoolean('isCollapsed');
  }

  resetValues = () => {
    localStorage.removeItem("minute")
    localStorage.removeItem("second")
    localStorage.removeItem("isStarted")
    localStorage.removeItem("isCollapsed")
    this.minute = 0;
    this.second = 0;
    this.minuteStr = this.updateTime(this.minute);
    this.secondStr = this.updateTime(this.second);
    this.isTinyVisible = false;
    this.isCollapsed = false;
    this.isStarted = false;
  }

  convertBooleanToStr = (val: boolean): string => {
    return (val == true) ? "true" : "false";
  }

  convertLocStorageToNumber = (key: string): number => {
    var i = localStorage.getItem(key)

    return (i == null) ? 0 : +i;
  }

  convertLocStorageToBoolean = (key: string): boolean => {
    var i = localStorage.getItem(key)

    return (i == null) ? false : true;
  }

  // API Call
  // Envoie des données afin de démarrer la study
  postStartStudies = (timerValue: string, labelsId: string) => {
    // Création du header
    const headers = new HttpHeaders ({
      "Authorization": this.auth.getToken()
    });

    const formData = new FormData();
    formData.append('timer', timerValue);
    formData.append('labelsId', labelsId);

    return this.http.post('http://localhost:5000/studies/start', formData, {headers});
  }

  // Appel POST confirmant l'activité étant toujours en cours
  postHeartbeatStudies = () => {
    // Création du header
    const headers = new HttpHeaders ({
      "Authorization": this.auth.getToken()
    });

    return this.http.post('http://localhost:5000/studies/heartbeat', {}, {headers});
  }

  // Appel informant l'arrêt / annulation de la study
  postStopStudies = () => {
    // Création du header
    const headers = new HttpHeaders ({
      "Authorization": this.auth.getToken()
    });

    return this.http.post('http://localhost:5000/studies/stop', {}, {headers});
  }
}
