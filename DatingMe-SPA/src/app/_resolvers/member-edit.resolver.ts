import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IUser } from '../_models/user';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<IUser> {
    constructor(
        private userService: UserService, private authService: AuthService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<IUser> {

        return this.userService.getUserById(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('مشکل دربازیابی اطلاعات');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
