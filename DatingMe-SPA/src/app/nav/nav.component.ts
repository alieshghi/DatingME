import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authServic: AuthService) { }
  ngOnInit(): void {

  }
  login(){
    this.authServic.login(this.model).subscribe(next => {
      console.log('you are login');
    } , error => {
      console.log(error);
    });
  }
  logedIn(){
    const token = localStorage.getItem('token');
    return !! token;
  }
  logedOut(){
    localStorage.removeItem('token');
    console.log('loged out');
  }

}
