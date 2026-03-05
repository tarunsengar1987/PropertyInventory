import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotfoundComponent } from './pages/notfound/notfound.component';
import { AppLayoutComponent } from './layout/app.layout.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: '',
                component: AppLayoutComponent,
                children: [
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    {
                        path: 'dashboard',
                        loadChildren: () =>
                            import('./pages/dashboard/dashboard.module')
                                .then(m => m.DashboardModule)
                    },
                    {
                        path: 'property-list',
                        loadChildren: () =>
                            import('./pages/property-list/property-list.module')
                                .then(m => m.PropertyListModule)
                    },
                    {
                        path: 'contacts',
                        loadChildren: () =>
                            import('./pages/contacts/contacts.module')
                                .then(m => m.ContactsModule)
                    }
                ]
            },
            { path: 'notfound', component: NotfoundComponent },
            { path: '**', redirectTo: '/notfound' }
        ],
            { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }