import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ILabelModel} from 'src/app/models/ILabelModel';
import {AuthService} from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  constructor(private http: HttpClient, private auth: AuthService) {
  }

  createLabels(label: string): Observable<ILabelModel> {
    const headers = new HttpHeaders({
      "Authorization": `${this.auth.getToken()}`
    });

    const formLabel = new FormData();
    formLabel.append('labelName', label);

    return this.http.post<ILabelModel>(`http://localhost:5000/Label/create`, formLabel, {headers});
  }

  updateLabel(labelId: number, labelName: string) {
    const headers = new HttpHeaders({
      "Authorization": `${this.auth.getToken()}`
    });

    const formLabel = new FormData();
    formLabel.append('labelName', labelName);

    return this.http.put(`http://localhost:5000/Label/update/${labelId}`, formLabel, {headers});
  }


  labelDelete(labelid: number) {
    const headers = new HttpHeaders({
      "Authorization": `${this.auth.getToken()}`
    });
    return this.http.delete<ILabelModel>(`http://localhost:5000/Label/delete/${labelid}`, {headers});
  }

  getLabels = () => {
    // Création du header
    const headers = new HttpHeaders({
      "Content-Type": "application/json",
      "Authorization": `${this.auth.getToken()}`
    });

    return this.http
      .get<ILabelModel[]>('http://localhost:5000/Label/me', {headers})
  }
}
