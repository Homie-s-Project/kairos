import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {IGroupModel} from 'src/app/models/IGroupModel';
import {AuthService} from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient,
              private authService: AuthService) {
  }

  createGroups(group: IGroupModel): Observable<IGroupModel> {
    const headers = new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": this.authService.getToken()
    });

    const formData = new FormData();
    formData.append('name', group.groupName);

    return this.http.post<IGroupModel>('http://localhost:5000/Group/create', formData, {headers})
  }


  groupDelete(groupId: number): Observable<IGroupModel> {
    const headers = new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": this.authService.getToken()
    });

    return this.http
      .delete<IGroupModel>('http://localhost:5000/Group/delete/'+groupId, {headers})
  }

  getGroups(): Observable<IGroupModel[]> {
    const headers = new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": this.authService.getToken()
    });

    return this.http
      .get<IGroupModel[]>('http://localhost:5000/Group/personal', {headers})
  }
}
