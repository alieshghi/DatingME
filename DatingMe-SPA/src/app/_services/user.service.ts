import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiurl;
  constructor(private http: HttpClient) {
   }
   getUsers(): Observable<IUser[]> {
    return this.http.get<IUser[]>(this.baseUrl + 'users');
  }
  getUserById(id: number): Observable<IUser> {
    return this.http.get<IUser>(this.baseUrl + 'users/' + id);
  }
  saveUser(id: number, user: IUser){
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }
}
