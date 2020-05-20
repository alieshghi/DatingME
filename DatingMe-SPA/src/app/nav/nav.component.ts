import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public authServic: AuthService, private alertify: AlertifyService, private router: Router) { }
  ngOnInit(): void {

  }
  login(){
    this.authServic.login(this.model).subscribe(next => {
      this.alertify.success('خوش اومدی');
    } , error => {
      this.alertify.error(error.error);
      console.log(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }
  logedIn(){
    return this.authServic.logedIn();
  }
  logedOut(){
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }

}
