import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PropertyListComponent } from './property-list.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', component: PropertyListComponent },
    ])],
    exports: [RouterModule]
})
export class PropertyListRoutingModule { }
