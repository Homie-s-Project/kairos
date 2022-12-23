import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class TimerService {
  base: any;
  minuteStr: string;
  secondStr: string;
  minute: number = 0;
  second: number = 0;
  isTinyVisible: boolean = true;
  isStarted: boolean = false;
  isCollapsed: boolean = false;

  constructor(private http: HttpClient, private auth: AuthService) {
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
      throw new Error("Valeur du temps à 0");
    }

    this.postStartStudies((this.minute + this.second).toString(), "")
      .subscribe(response => {
        console.log(response)
      })

    this.base = setInterval(this.timerCountdown, 1000);
  }

  timerCountdown = () => {
    this.second--;

    if (this.second < 0) {
      this.minute--;

      if (this.minute <= 0 && this.second <= 0) {
        this.stopCountdown();
        return;
      }

      this.second = 59;
    }

    this.saveValues();
    this.minuteStr = this.updateTime(this.minute);
    this.secondStr = this.updateTime(this.second);
  }

  stopCountdown = () => {
    clearInterval(this.base);
    this.resetValues()
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
  postStartStudies = (timerValue: string, labelsId: string) => {
    // Création du header
    const headers = new HttpHeaders ({
      "Content-Type": "application/json",
      "Authorization": `${this.auth.getToken()}`
    })

    const data = {
      timer: timerValue,
      labelsId: labelsId
    }

    return this.http.post('http://localhost:5000/studies/start', data, {headers})
  }
}
