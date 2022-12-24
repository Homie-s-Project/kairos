import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Injectable, OnInit} from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UserModel } from 'src/app/models/user.model';
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly token?: string;

  constructor(private http: HttpClient,
              private cookie: CookieService) {
    this.token = this.cookie.get('jwt');
  }

  // Contrôle du Token JWT
  isLoggedIn(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      if (this.token) {
        // Création du header
        const header = new HttpHeaders ({
          "Content-Type": "application/json",
          "Authorization": `${this.getToken()}`
        });

        this.http
          .get('http://localhost:5000/User/me', {
            headers: header,
            observe: 'response',
          })
          .subscribe(
            response => {
              resolve(response.status === 200);
            },
            error => {
              reject(error);
            }
          );
      } else {
        resolve(false);
      }
    });
  }

  // Récupèration de l'user selon le Token JWT
  getProfile(): Observable<HttpResponse<UserModel>> {
    const header = new HttpHeaders ({
      "Content-Type": "application/json",
      "Authorization": `${this.getToken()}`
    });

    return this.http.get<UserModel>('http://localhost:5000/User/me', {headers: header, observe: 'response'});
  }

  // Retourne Bearer Token
  getToken() :string {
      return `Bearer ${this.token}`;
  }

  // Supprime le JWT des cookies
  logout(): void {
    this.cookie.delete('jwt');
  }
}
