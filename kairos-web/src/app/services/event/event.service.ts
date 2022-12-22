import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {IGroupModel} from "../../models/IGroupModel";

function getOauth() {

  let t = "ho6g1ytIOz5V61AmDmoZIlzI+M09awac1zPLDG5+erWJ6wgUVdG1oOTvmY1B0pMQFHBsYkbMcPQYuzKf2MsWRgZIFkvVscVOnbTQnvH765uCEDY061+T4G+YMEYrkCe53TBeyQN3WLy5+D5lalCnH8jZl8F1VEr2KG7qOdjx+11Bm+5PgJeUggdGa4oipf/U2tpsUk19snnMI9pbMWfUOT+dHi3AImR+v50WQWQgk7pTwrgF4xMCAtwU+3qoZ3zFAcAtat3YIOlA+91iJAwktZDlY/rzwU8B6oJO34SCBiS5RzBII49ekPGwtDlRzItyEo8k9IU8njLm4m5UEON7DUyzE1w5Uz2Tp5iIs4mablicdHbcjBhp7YZ8MPLHXxWYnJB/FFAPSpONfxHl8pQa5fNYePFV6G0zPsNhz5E9E8BCSswWcwzJI3j51umo69bOM6hWR/+pZNtgyLurppLFPpA+u0fWZSfwUtu3xr5+hBeDjDr5Qg7u8jHXzW/LT1go"

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
}
