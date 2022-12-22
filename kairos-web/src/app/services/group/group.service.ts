import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import{Observable } from 'rxjs';
import { IGroupModel } from 'src/app/models/IGroupModel';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  groupUrl="api/groups"
  
    constructor( private http:HttpClient) { }
    GetGroupById(groupid:string){
      return this.http.get<IGroupModel>(this.groupUrl+"/"+groupid)
    }
  
    CreateGroups(group:IGroupModel):Observable<IGroupModel>{
      let httpheaders=new HttpHeaders()
      .set('Content-type','application/Json');
      let options={
        headers:httpheaders
      };
      return this.http.post<IGroupModel>(this.groupUrl,group,options);
    }
  
  
    GroupDelete (groupid:number):Observable<number>{
      let httpheaders=new HttpHeaders()
      .set('Content-type','application/Json');
      let options={
        headers:httpheaders
      };
      return this.http.delete<number>(this.groupUrl+"/"+groupid);
    }
  
    GetGroups():Observable<IGroupModel[]>{
      return this.http.get<IGroupModel[]>(this.groupUrl);
    }
}