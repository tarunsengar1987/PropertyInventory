import { Component, ElementRef, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { Router } from '@angular/router';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent {

    items: MenuItem[] | undefined;

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(public layoutService: LayoutService, private _router: Router) { }

    ngOnInit() {
        this.items = [
            {
                items: [
                    {
                        label: 'Profile',
                        icon: 'pi pi-user',
                        routerLink: '/profile'
                    },
                    {
                        label: 'Sign out',
                        icon: 'pi pi-sign-out',
                        command: () => {
                            sessionStorage.clear();
                            this._router.navigate(['/auth/sign-in']);
                        }
                    }
                ]
            },
        ];
    }
}
