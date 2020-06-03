import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { IUser } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {

  jwtService = new JwtHelperService();
  constructor(public authServic: AuthService, private elementRef: ElementRef) {
  }
  ngAfterViewInit(): void {
    this.elementRef.nativeElement.ownerDocument.body.style.backgroundColor = 'pink';
  }
  ngOnInit(){
   const token = localStorage.getItem('token');
   const user: IUser = JSON.parse(localStorage.getItem('user'));
   if (token) {
     this.authServic.decodedToken = this.jwtService.decodeToken(token);
   }
   if (user) {
     this.authServic.user = user;
     this.authServic.changeMemberPhoto(user.photoUrl);
   }
  }
}
