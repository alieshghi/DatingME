import { Component, OnInit } from '@angular/core';
import { IMessage } from '../_models/message';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { IPagination } from '../_models/Pagination';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: IMessage[];
  pagination: IPagination;
  messageContainer = 'unRead';
  constructor( private activeHttp: ActivatedRoute, private userService: UserService,
               private authService: AuthService, private alertify: AlertifyService ) { }

  ngOnInit(): void {
    const messages = 'messages';
    this.activeHttp.data.subscribe((data) => {
      console.log(data);
      this.messages = data[messages].pageResult;
      this.pagination = data[messages].paginationResult;
    });
  }
  loadMessages(){
    this.userService.getMessages(this.authService.decodedToken.nameid, this.pagination.curentPage,
     this.pagination.pageSize, this.messageContainer).subscribe(res => {
     this.messages = res.pageResult,
     this.pagination = res.paginationResult;
     }, error => { this.alertify.error(error);
    });
  }
  deleteMessage(messageId: number){
    this.alertify.confirm('آیااز حذف مطمئنی؟', () => {
      this.userService.deleteMessage(this.authService.decodedToken.nameid, messageId).subscribe(res => {
        this.messages.splice(this.messages.findIndex(x => x.id === messageId), 1);
        this.alertify.success('حذف شد');
      });
    });
  }
  pageChanged(event: any): void {
    this.pagination.curentPage = event.page;
    this.loadMessages();
  }
}
