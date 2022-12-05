import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UserModel } from 'src/app/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user: any;

  //constructor() { }
  constructor(private cookie: CookieService, private http: HttpClient) { }

  isLoggedIn = (): boolean => {
    var auth_token = this.cookie.get('jwt');
    var isLogged = true;
    
    // if (auth_token != "") {
      
    //   return true;
    // }
    const headers = { headers: new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": `Bearer ${auth_token}`
    })};

    this.http
    .get('http://localhost:5000/User/me', headers)
    .subscribe(response => {
      this.user = response;
      console.log(this.user);
    }, error => {
      isLogged = false;
      console.log(error.status);
      console.log(error.statusText);
    });
    
    return isLogged;
  }

  // A finaliser et utiliser pour la page de profil
  // getProfileAsync = (): any => {
  //   var currentUser: any;
  //   const auth_token = this.cookie.get('jwt')
  //   const headers = new HttpHeaders ( {
  //     "Content-Type": "application/json",
  //     "Authorization": `Bearer ${auth_token}`
  //   });-
    
  //   const requestOptions = { headers: headers };

  //   this.http
  //     .get('http://localhost:5000/User/me', requestOptions)
  //     .subscribe(response => {
  //       currentUser = response;
  //       console.log(currentUser)
  //     })

  //   return (currentUser)
  // }
}
