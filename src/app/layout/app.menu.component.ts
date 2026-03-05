import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                items: [
                    { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] },
                    { label: 'Contacts', icon: 'pi pi-fw pi-id-card', routerLink: ['/contacts'] },
                    { label: 'Property', icon: 'pi pi-fw pi-id-card', routerLink: ['/property-list'] },
                ]
            }
        ];
    }
}
