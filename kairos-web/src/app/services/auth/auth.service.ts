import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private token: string;

  constructor(private http: HttpClient, private cookie: CookieService) {
    this.token = this.cookie.get('jwt');
  }

  isLoggedIn = (): Promise<boolean> => {
    return new Promise((resolve, reject) => {
      if (this.token) {
        // Création du header
        const header = new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: `Bearer ${this.token}`,
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

  getProfile = () => {
    const header = new HttpHeaders ({
      "Content-Type": "application/json",
      "Authorization": `Bearer ${this.token}`
    });

    this.http
      .get('http://localhost:5000/User/me', {headers: header, observe: 'response'})
      .subscribe(response => {
        console.log(response);
      });
  }
}
