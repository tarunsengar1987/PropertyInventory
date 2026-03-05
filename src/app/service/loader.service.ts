import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class LoaderService {
    _isLoading$ = new BehaviorSubject<boolean>(false);
    show() { 
        this._isLoading$.next(true); }

    hide() { this._isLoading$.next(false); }
}