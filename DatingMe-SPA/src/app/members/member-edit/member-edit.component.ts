import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { IUser } from 'src/app/_models/user';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: IUser;
    @ViewChild('memberEditForm', {static: true}) memberEditForm: NgForm;

  constructor(
    private router: ActivatedRoute, private alertify: AlertifyService, private userService: UserService, private authService: AuthService
    ) { }

  ngOnInit(): void {
    const singleuser = 'user';
    this.router.data.subscribe(data => {
      this.user = data[singleuser];
    });
  }
  save(){
    const id = this.authService.decodedToken.nameid;
    this.userService.saveUser(id , this.user).subscribe(res => {
      this.alertify.success('اطلاعات ثبت شد!');
      this.memberEditForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });

  }

}
