import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { IUser } from '../_models/user';
import { ListResolver } from '../_resolvers/list.resolver';
import { IPagination, PaginationResult } from '../_models/Pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: IUser[];
  pagedList: IPagination;
  likeFilter: string;
  constructor(private activatedRoute: ActivatedRoute,
              private userService: UserService, private alertifye: AlertifyService,
              ) { }

  ngOnInit(): void {
    const users = 'users';
    this.activatedRoute.data.subscribe((data) => {
      this.users = data[users].pageResult;
      this.pagedList = data[users].paginationResult;
    }, error =>
    console.log(error));
    this.likeFilter = 'Likers';
  }
  getUserLikedOrLiker(){
    this.userService.getUsers(this.pagedList.curentPage, this.pagedList.pageSize, null, this.likeFilter).subscribe(
      (data: PaginationResult<IUser[]>) => {
               this.users = data.pageResult;
               this.pagedList = data.paginationResult;
              }, error =>
              console.log(error));
  }
  pageChanged(event: any): void {
    this.pagedList.curentPage = event.page;
    this.getUserLikedOrLiker();
  }

}
