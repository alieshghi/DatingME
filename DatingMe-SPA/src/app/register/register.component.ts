import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();
  model: any = {};
  constructor(private autServic: AuthService) { }

  ngOnInit(): void {
  }
  register(){
    console.log(this.model);
    this.autServic.register(this.model).subscribe(respons =>
      console.log(respons), error =>
      console.log(error));
  }
  cancel(){
    this.cancelRegistration.emit(false);
  }
}
