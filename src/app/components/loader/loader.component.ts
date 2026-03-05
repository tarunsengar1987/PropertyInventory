import { Component } from '@angular/core';
import { LoaderService } from 'src/app/service/loader.service';

@Component({
    selector: 'app-loader',
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.style.scss'],
})

export class LoaderComponent {
    show: boolean = false;
    
    constructor(private _loaderService: LoaderService) {
        this._loaderService._isLoading$.subscribe(res => { this.show = res; });
    }
}
