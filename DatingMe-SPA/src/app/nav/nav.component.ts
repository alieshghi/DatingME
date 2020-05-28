import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { UserService } from '../_services/user.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;
  constructor(public authServic: AuthService, private alertify: AlertifyService, private router: Router) { }
  ngOnInit(): void {
    this.authServic.currentPhotoUrl.subscribe( photoUrl => {
      this.photoUrl = photoUrl;
    });
  }
  login(){
    this.authServic.login(this.model).subscribe(next => {
      this.alertify.success('خوش اومدی');
    } , error => {
      this.alertify.error(error.error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }
  logedIn(){
    return this.authServic.logedIn();
  }
  logedOut(){
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authServic.user = null;
    this.authServic.decodedToken = null;
    this.alertify.message('شما از کاربری خود خارج شدید');
    this.router.navigate(['/home']);
  }

}
