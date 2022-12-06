import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UserModel } from 'src/app/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  //constructor() { }
  constructor(private cookie: CookieService, private http: HttpClient) { }

  isLoggedIn = (): boolean => {
    if (this.cookie.get('jwt') != "") {
      return true;
    }
    
    return false;
  }

  // getProfileAsync = (): any => {
  //   var currentUser: any;
  //   const auth_token = this.cookie.get('jwt')
  //   const headers = new HttpHeaders ( {
  //     "Content-Type": "application/json",
  //     "Authorization": `Bearer ${auth_token}`
  //   })
    
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
