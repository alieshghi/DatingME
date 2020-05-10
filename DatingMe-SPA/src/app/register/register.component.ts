import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService} from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();
  model: any = {};
  constructor(private autServic: AuthService, private alertify: AlertifyService ) { }

  ngOnInit(): void {
  }
  register(){
    this.autServic.register(this.model).subscribe(respons => {
      this.alertify.success('حساب  کاربری شما ایجاد شد');
    }
      , error =>
      {
        this.alertify.error(error.error);
      });
        }
  cancel(){
    this.cancelRegistration.emit(false);
  }
}
