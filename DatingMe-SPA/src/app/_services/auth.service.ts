import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { map } from 'rxjs/Operators';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/auth/';
  constructor(private http: HttpClient) { }
  login(model: any){
    return this.http.post(this.baseUrl + 'login', model).pipe(map((response: any) => {
      const user = response;
      localStorage.setItem('token', user.token);
    }));
}
register(model: any){
return this.http.post(this.baseUrl + 'register', model);
}
}
