import { Component, OnInit, Input } from '@angular/core';
import { IUser } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: IUser;
  currentId: number;
  constructor(private userService: UserService, private authService: AuthService,
              private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.currentId = this.authService.decodedToken.nameid;
  }
  createLike(rescievdId: number){
    this.userService.createLike(this.currentId, rescievdId).subscribe(next => {
      this.alertify.success('پسندیده شد');
    }, error => {
      this.alertify.error(error.error);
    }
    );
  }
}
