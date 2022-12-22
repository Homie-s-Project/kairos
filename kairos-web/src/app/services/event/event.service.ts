import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGroupModel} from "../../models/IGroupModel";
import {IEventModel} from "../../models/IEventModel";
import {AuthService} from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  getEvent() : Observable<IGroupModel[]>{
    const header = new HttpHeaders ({
      "Authorization": this.authService.getToken()
    });

    return this.http.get<IGroupModel[]>('http://localhost:5000/Event/me', {headers: header});
  }

  createEvent(event: IEventForm) : Observable<IEventModel> {
    const headers = new HttpHeaders({
      "Authorization": this.authService.getToken()
    });

    const formData = new FormData();
    formData.append('groupId', event.group);
    formData.append('labels', event.label);
    formData.append('title', event.title);
    formData.append('description', event.description);
    formData.append('sessionDate', event.date.toString());

    return this.http.post<IEventModel>('http://localhost:5000/Event/create', formData, {headers});
  };
}

export interface IEventForm {
  group: string;
  label: string;
  title: string;
  description: string;
  date: Date;
}
