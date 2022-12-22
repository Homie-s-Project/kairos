import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGroupModel} from "../../models/IGroupModel";
import {IEventModel} from "../../models/IEventModel";

function getOauth() {

  let t = "ho6g1ytIOz5V61AmDmoZIlzI+M09awac1zPLDG5+erWeC0n06xdFrt/RlVdiaDEqFHBsYkbMcPQYuzKf2MsWRgZIFkvVscVOnbTQnvH765uCEDY061+T4G+YMEYrkCe53TBeyQN3WLy5+D5lalCnH8jZl8F1VEr2KG7qOdjx+11b5N1PqrX4VcV1B9aiEBbvKmfKHUnX/OZlrJ+c1vSJYcsVaiv7iwt4w2qOjaJh7yONC6w3NIzVlq2WXWUDq36IW1W9TGlW2H7lgscfPYUh+mv1XdMuFgiSf+d/Y3oXAkbdBfqRBMVLIVdp+cmHLmWnIsdR4krBl5cA6xnnktsZ/AGlejZIPG4irQDa0Fu+M3ycdHbcjBhp7YZ8MPLHXxWYnJB/FFAPSpONfxHl8pQa5fNYePFV6G0zPsNhz5E9E8C5gLgLGms15N6MRvJFoMEqUh+4Xz+b8kFuZT0AsEkyr1TsyVq8+47F7UGVQh9JW2mIENfpaFhZjbVKRxCMeKLG"

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

  createEvent(event: IEventForm) : Observable<IEventModel> {
    const headers = new HttpHeaders({
      "Content-Type": "application",
      "Authorization": `Bearer ` + getOauth()
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
