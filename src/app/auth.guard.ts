import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private _router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot,):
        | Observable<boolean | UrlTree>
        | Promise<boolean | UrlTree>
        | boolean
        | UrlTree {
        const token = sessionStorage.getItem('token');

        if (state.url.includes('auth/sign-in')) {
            if (!token) {
                return true;
            } else {
                this._router.navigate(['/dashboard']);
                return false;
            }
        } else {
            if (token) {
                return true;
            }
            this._router.navigate(['/auth/sign-in']);
            return false;
        }
    }
}