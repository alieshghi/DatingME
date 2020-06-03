import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { IUser } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { PaginationResult, IPagination } from 'src/app/_models/Pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: IUser[];
  pagedList: IPagination;
  user: IUser = JSON.parse(localStorage.getItem('user'));
  userParams: any = {};
  genderList = [ {value: '', display: 'هر دو'}, {value: 'male', display: 'مرد'}, {value: 'female', display: 'زن'}];
  sortOrderList = [{value: '', display: 'آخرین ورود به سایت'}, { value: 'createdDate', display: 'تاریخ ثبت نام'}
                  , {value: 'age', display: 'سن'}];
  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute ) { }

  ngOnInit(): void {
    const users = 'users';
    this.route.data.subscribe((data) => {
      this.users = data[users].pageResult;
      this.pagedList = data[users].paginationResult;
    });
    this.userParams.maxAge = 99;
    this.userParams.minAge = 18;
    this.userParams.city = '';
    this.userParams.countery = '';
    this.userParams.gender = '';
    this.userParams.sortOrder = '';
    this.userParams.sortType = '0';
  }
  pageChanged(event: any): void {
    this.pagedList.curentPage = event.page;
    this.loadUsers();
  }
  loadUsers() {
    this.userService
      .getUsers(this.pagedList.curentPage, this.pagedList.pageSize, this.userParams)
      .subscribe((res: PaginationResult<IUser[]>) => {
        this.users = res.pageResult;
        this.pagedList = res.paginationResult;
    }, error => {
      this.alertify.error(error);
    });
  }
reset(){
  this.userParams.maxAge = 99;
  this.userParams.minAge = 18;
  this.userParams.gender = '';
  this.userParams.city = '';
  this.userParams.countery = '';
  this.userParams.sortOrder = '';
  this.userParams.sortType = '0';
  this.loadUsers();
}

}

