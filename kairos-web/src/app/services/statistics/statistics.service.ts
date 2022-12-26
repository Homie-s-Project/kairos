import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthService} from "../auth/auth.service";
import {environment} from "../../../environments/environment";

export interface IHoursStudiedWeekModel {
  dayOfWeek: string[];
  hours: number[];
}

export interface IHoursStudiedWeekLabelModel {
  labels: string[];
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

    return this.http.get<number>(`${environment.apiUrl}/studies/lastWeek/rate`, {headers});
  }

  getHoursStudied(): Observable<IHoursStudiedWeekModel>{
    let headers = new HttpHeaders({
      'Authorization': this.authService.getToken()
    });

    return this.http.get<IHoursStudiedWeekModel>(`${environment.apiUrl}/studies/lastWeek/hoursStudied`, {headers});
  }

  getHoursPerLabel(): Observable<IHoursStudiedWeekLabelModel>{
    let headers = new HttpHeaders({
      'Authorization': this.authService.getToken()
    });

    return this.http.get<IHoursStudiedWeekLabelModel>(`${environment.apiUrl}/studies/lastWeek/hoursPerLabel`, {headers});
  }
}
