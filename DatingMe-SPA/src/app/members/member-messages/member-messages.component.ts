import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { IMessage } from 'src/app/_models/message';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/Operators';


@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: IMessage[];
  newMessage: any = {};
  constructor(private authService: AuthService, private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.getAll();
  }
  getAll(){
    const userId = + this.authService.decodedToken.nameid;
    this.userService.gethMessageThread(this.authService.decodedToken.nameid,
       this.recipientId).pipe
       (tap(message => {
         // tslint:disable-next-line: prefer-for-of
         for (let i = 0; i < message.length; i++) {
           if (message[i].isRead === false && message[i].recipientId === userId) {
             this.userService.markAsRead(userId, message[i].id);
           }
         }
       }))
    .subscribe(
      next => {
        this.messages = next;
      }, error => {
        this.alertify.error(error);
      }
    );
  }
  sendMessage(){
    this.newMessage.recipientId = this.recipientId;
    this.userService.createMessage(this.authService.decodedToken.nameid, this.newMessage).subscribe(
      (message: IMessage) => {
        this.messages.unshift(message);
        this.newMessage.content = '';
      }, error => {
        this.alertify.error(error);
      }
    );
  }

}
