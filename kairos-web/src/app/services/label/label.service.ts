import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ILabelModel } from 'src/app/models/ILabelModel';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  labelUrl="api/labels"
  
    constructor( private http:HttpClient) { }
    GetLabelById(labelid:string){
      return this.http.get<ILabelModel>(this.labelUrl+"/"+labelid)
    }
  
    CreateLabels(label:ILabelModel):Observable<ILabelModel>{
      let httpheaders=new HttpHeaders()
      .set('Content-type','application/Json');
      let options={
        headers:httpheaders
      };
      return this.http.post<ILabelModel>(this.labelUrl,label,options);
    }
  
  
    LabelDelete (labelid:number):Observable<number>{
      let httpheaders=new HttpHeaders()
      .set('Content-type','application/Json');
      let options={
        headers:httpheaders
      };
      return this.http.delete<number>(this.labelUrl+"/"+labelid);
    }
  
    GetLabels():Observable<ILabelModel[]>{
      return this.http.get<ILabelModel[]>(this.labelUrl);
    }
}
