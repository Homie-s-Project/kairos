import { normalizePassiveListenerOptions } from '@angular/cdk/platform';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  firstDay: Date = new Date();
  lastDay: Date = new Date();

  constructor() { }

  getCurrentWeek = ():string => {
    const d = new Date();
    var day = d.getDay(), 
      diff = d.getDate() - day + (day == 0 ? -6:1);
    this.lastDay = new Date(d.setDate(diff + 6));
    this.firstDay = new Date(d.setDate(diff));

    return `(semaine du ${this.formatDate(this.firstDay)} au ${this.formatDate(this.lastDay)})`;
  }

  formatDate = (date: Date) => {
    return [
      this.padTo2Digits(date.getDate()),
      this.padTo2Digits(date.getMonth() + 1),
      date.getFullYear()
    ].join('.');
  }

  padTo2Digits = (num: number): string => {
    return num.toString().padStart(2, '0');
  }
}
