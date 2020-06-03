import {Injectable} from '@angular/core';
import {IUser} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PaginationResult } from '../_models/Pagination';

@Injectable()
export class MemberListResolver implements Resolve<PaginationResult<IUser[]>> {
    pageNumber = 1;
    pageSize = 5;

    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<IUser[]>> {
        return this.userService.getUsers(this.pageNumber.toString(), this.pageSize.toString()).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
