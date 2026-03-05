import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { forkJoin } from 'rxjs';
import { ContactService } from 'src/app/service/contact.service';
import { DashboardService } from 'src/app/service/dashboard.service';
import { PropertyService } from 'src/app/service/property.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  displayedColumns = ['propertyName', 'owner', 'dateOfPurchase', 'askingPrice', 'soldAtEUR', 'soldAtUSD'];

  dataSource = new MatTableDataSource<any>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  totalProperties = 0;
  totalContacts = 0;
  totalSales = 0;
  isLoading = true;
  hasError = false;

  constructor(
    private dashboardService: DashboardService,
    private propertyService: PropertyService,
    private contactService: ContactService
  ) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.hasError = false;

    forkJoin({
      sales: this.dashboardService.getSales(),
      properties: this.propertyService.getAll(),
      contacts: this.contactService.getAll()
    }).subscribe({
      next: (results: any) => {
        const sales = results.sales?.data ?? [];
        this.dataSource.data = sales;
        this.totalSales = sales.length;
        this.totalProperties = results.properties?.data?.items?.length
          ?? results.properties?.data?.length ?? 0;
        this.totalContacts = results.contacts?.data?.items?.length
          ?? results.contacts?.data?.length ?? 0;
        this.isLoading = false;
        setTimeout(() => { this.dataSource.paginator = this.paginator; });
      },
      error: () => {
        this.isLoading = false;
        this.hasError = true;
      }
    });
  }
}
