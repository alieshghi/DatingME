import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService} from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { IUser } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();
  user: IUser;
  registerForm: FormGroup;
  bsDateConfig: Partial<BsDatepickerConfig>;
  constructor(private autServic: AuthService, private router: Router, private alertify: AlertifyService, private fb: FormBuilder ) { }

  ngOnInit(): void {
    this.creatform();
    this.bsDateConfig = {
      containerClass: 'theme-red'
    };
    // this.registerForm = new FormGroup({
    //   userName: new FormControl('', [Validators.required]),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', [Validators.required]),
    // }, [this.checkDisMatch] );
  }
  creatform(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      userName: ['', [Validators.required]],
      knownAs: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required]]
    }, {validators: this.checkDisMatch});
  }
  register(){
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.autServic.register(this.user).subscribe(() => {
        this.alertify.success('حساب  کاربری شما ایجاد شد');
      } , error =>
        {
          console.log(error);
          this.alertify.error(error);
        }, () => {this.autServic.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      }
        );
    }
        }
  checkDisMatch(dom: FormGroup){
    if (dom.get('password').value === dom.get('confirmPassword').value) {
      return null;
    }
    else{
      return {mistmatch: true};
    }
  }
  cancel(){
    this.cancelRegistration.emit(false);
  }
}
