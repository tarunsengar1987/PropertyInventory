import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, catchError, finalize, throwError } from "rxjs";
import { LoaderService } from "./service/loader.service";
import { NotificationService } from "./service/notification.service";
import { Router } from "@angular/router";

@Injectable({
    providedIn: 'root'
})

export class RequestInterceptor implements HttpInterceptor {

    constructor(private _loaderService: LoaderService, private _notificationService: NotificationService, private _router: Router) { }

    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        this._loaderService.show()
        const token = sessionStorage.getItem('token');
        let cloned = request;
        if (token) cloned = request.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
        return next.handle(cloned).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status == 401 && !request.url.includes('auth')) {
                    sessionStorage.clear();
                    setTimeout(() => { this._router.navigate(['/auth/sign-in']); }, 1000);
                }
                this._notificationService.error(error?.error?.message || error?.message)
                return throwError(error?.error?.message || error?.message);
            }),
            finalize(() => this._loaderService.hide()));
    }
}