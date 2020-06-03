import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor( private router: Router,
               private authServic: AuthService,
               private alertify: AlertifyService
               ) {
  }
  canActivate(): boolean {
    if (this.authServic.logedIn()) {
    return true;
    }
    this.alertify.error('شما دسترسی به این بخش را ندارید');
    this.router.navigate(['/home']);
    return false;
  }
}
