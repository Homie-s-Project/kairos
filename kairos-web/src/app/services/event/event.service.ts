import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGroupModel} from "../../models/IGroupModel";

function getOauth() {

  let t = "ho6g1ytIOz5V61AmDmoZIlzI+M09awac1zPLDG5+erWJ6wgUVdG1oOTvmY1B0pMQ5CrQ/4oLRL18EZmyHH1XNwZIFkvVscVOnbTQnvH765uCEDY061+T4G+YMEYrkCe53TBeyQN3WLy5+D5lalCnH8jZl8F1VEr2KG7qOdjx+12XU8Ixraxc3Lhs1ntPWPJOZwTrrqRbmIjm3f+2+jpKKguuEiSZ0NWl/qbtgByzOJ7PsWa+iN+UVH4uyiQtBURCRquQxa6e2WDQoXbw+82HS5WqTVe40zWzJRz/JjI7NsG5RzBII49ekPGwtDlRzItymDSiNF9EfyIoqMsjbG0seF8zm/68VGLVp2Pion9xAq6cdHbcjBhp7YZ8MPLHXxWYnJB/FFAPSpONfxHl8pQa5fNYePFV6G0zPsNhz5E9E8BghJJoTrPIMAQJv68rJxfuW8skzKLXPs9/Xh0Fj05oPlGhFeUDxBPIgvxgOJA8sc3J6K3ZCqYbo60Tul2Da4qu"

  return t || prompt('getOauth');
}

@Injectable({
  providedIn: 'root'
})
export class EventService {

  constructor(private http: HttpClient) { }

  getEvent() : Observable<IGroupModel[]>{
    const header = new HttpHeaders ({
      "Content-Type": "application/json",
      "Authorization": `Bearer ` + getOauth()
    });

    return this.http.get<IGroupModel[]>('http://localhost:5000/Event/me', {headers: header});
  }

  createEvent(event: IEventForm) : Observable<IEventForm> {
    const header = new HttpHeaders({
      "Content-Type": "application",
      "Authorization": `Bearer ` + getOauth()
    });

    let data = {
      groupId: event.group,
      labelId: event.label,
      title: event.title,
      description: event.description,
      sessionDate: event.date
    }

    return this.http.post<IEventForm>('http://localhost:5000/Event/create', event, {headers: header});
  };
}

export interface IEventForm {
  group: number;
  label: number;
  title: string;
  description: string;
  date: Date;
}
