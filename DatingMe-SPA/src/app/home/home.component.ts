import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit  {
  registerMode = false;
  logedInMode = false;
  constructor(private authServic: AuthService) { }

  ngOnInit(): void {
    if (this.authServic.logedIn()) {
      this.logedInMode = true;
      }
    else{
      this.logedInMode = false;
      }

  }
  registerToggle(){
    return this.registerMode = true ;
  }
  registerOut(regiser: boolean){
    this.registerMode = regiser ;
  }

}
