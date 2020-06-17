import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { map } from 'rxjs/Operators';
import {  JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { IUser } from '../_models/user';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiurl + 'auth/';
  user: IUser;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');

  currentPhotoUrl = this.photoUrl.asObservable();
  jwtService = new JwtHelperService();
  decodedToken: any ;
  constructor(private http: HttpClient) { }
  changeMemberPhoto(photoUrl: string){
    this.photoUrl.next(photoUrl);
  }
  login(model: any){
    return this.http.post(this.baseUrl + 'login', model).pipe(map((response: any) => {
      const user = response;
      if (user) {
        this.decodedToken = this.jwtService.decodeToken(user.token);
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user.user));
        this.user = user.user;
        this.changeMemberPhoto(this.user.photoUrl);
      }
    }));
}
register(user: IUser){
    return this.http.post(this.baseUrl + 'register', user);
}
logedIn(){
  const token = localStorage.getItem('token');
  return !this.jwtService.isTokenExpired(token);
}
}
