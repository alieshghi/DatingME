import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { map } from 'rxjs/Operators';
import {  JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/auth/';
  jwtService = new JwtHelperService();
  decodedToken: any ;
  constructor(private http: HttpClient) { }
  login(model: any){
    return this.http.post(this.baseUrl + 'login', model).pipe(map((response: any) => {
      const user = response;
      localStorage.setItem('token', user.token);
      if (user) {
        this.decodedToken = this.jwtService.decodeToken(user.token);
      }
    }));
}
register(model: any){
return this.http.post(this.baseUrl + 'register', model);
}
logedIn(){
  const token = localStorage.getItem('token');
  return !this.jwtService.isTokenExpired(token);
}
}
