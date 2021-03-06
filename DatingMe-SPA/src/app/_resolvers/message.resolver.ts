import {Injectable} from '@angular/core';
import {IUser} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PaginationResult } from '../_models/Pagination';
import { IMessage } from '../_models/message';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MessageResolver implements Resolve<PaginationResult<IMessage[]>> {
    pageNumber = 1;
    pageSize = 5;
    userId = this.auth.decodedToken.nameid;
    messageContainer = 'unRead';

    constructor(private userService: UserService,
                private router: Router, private alertify: AlertifyService,
                private auth: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<IMessage[]>> {
        return this.userService.
            getMessages(this.userId, this.pageNumber.toString(), this.pageSize.toString()).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
