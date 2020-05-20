import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IUser } from '../_models/user';

@Injectable()
export class MemberDetailResolver implements Resolve<IUser> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IUser> {
        const id = 'id';
        return this.userService.getUserById(route.params[id]).pipe(
            catchError(error => {
                this.alertify.error('مشکل دربازیابی اطلاعات');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
