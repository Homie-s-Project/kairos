import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ILabelModel } from 'src/app/models/ILabelModel';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  labelUrl="api/labels"
  
    constructor( private http:HttpClient, private auth: AuthService) { }

    getLabelById(labelid:string){
      return this.http.get<ILabelModel>(this.labelUrl+"/"+labelid)
    }
  
    createLabels(label:ILabelModel):Observable<ILabelModel>{
      const headers = new HttpHeaders ({
        "Content-Type": "application/json",
        "Authorization": `${this.auth.getToken()}`
      });
      const formLabel = new FormData();
      formLabel.append('labelName', label.labelTitle);
    
      return this.http.post<ILabelModel>(`http://localhost:5000/Label/create`, formLabel, {headers});
    }
    
    updateLabel(labelid:number) {
      const headers = new HttpHeaders ({
        "Content-Type": "application/json",
        "Authorization": `${this.auth.getToken()}`
      });
      return this.http.put(`http://localhost:5000/Label/update/${labelid}`, {headers});
    }
  
  
    labelDelete (labelid:number){
      const headers = new HttpHeaders ({
        "Content-Type": "application/json",
        "Authorization": `${this.auth.getToken()}`
      });
      return this.http.delete<ILabelModel>(`http://localhost:5000/Label/delete/${labelid}`, {headers});
    }
  
    getLabels = () => {
      // Création du header
      const headers = new HttpHeaders ({
        "Content-Type": "application/json",
        "Authorization": `${this.auth.getToken()}`
      });
    
      return this.http
        .get<ILabelModel[]>('http://localhost:5000/Label/me', {headers})
    }
}
