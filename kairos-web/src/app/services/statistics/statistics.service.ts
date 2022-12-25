import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthService} from "../auth/auth.service";

export interface IHoursStudiedWeekModel {
  dayOfWeek: string[];
  hours: number[];
}

export interface IHoursStudiedWeekLabelModel {
  label: string[];
  hours: number[];
}

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  firstDay: Date = new Date();
  lastDay: Date = new Date();

  constructor(private http: HttpClient,
              private authService: AuthService) { }

  getCurrentRate(): Observable<number>{
    let headers = new HttpHeaders({
      'Authorization': this.authService.getToken()
    });

    return this.http.get<number>('http://localhost:5000/studies/lastWeek/rate', {headers});
  }

  getHoursStudied(): Observable<IHoursStudiedWeekModel>{
    let headers = new HttpHeaders({
      'Authorization': this.authService.getToken()
    });

    return this.http.get<IHoursStudiedWeekModel>('http://localhost:5000/studies/lastWeek/hoursStudied', {headers});
  }

  getHoursPerLabel(): Observable<IHoursStudiedWeekLabelModel>{
    let headers = new HttpHeaders({
      'Authorization': this.authService.getToken()
    });

    return this.http.get<IHoursStudiedWeekLabelModel>('http://localhost:5000/studies/lastWeek/hoursPerLabel', {headers});
  }
}
