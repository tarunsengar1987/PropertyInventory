import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PropertyService } from 'src/app/service/property.service';
import { ContactService } from 'src/app/service/contact.service';
import { Property } from 'src/app/model/property.model';
import { Contact } from 'src/app/model/contact.model';

type PanelMode = 'add' | 'edit' | 'transfer' | 'updatePrice' | null;

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.scss'],
})
export class PropertyListComponent implements OnInit {

  displayedColumns: string[] = ['name', 'address', 'currentPrice', 'dateOfRegistration', 'actions'];
  dataSource = new MatTableDataSource<Property>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  panelMode: PanelMode = null;
  selectedProperty: Property | null = null;
  isSubmitting = false;

  propertyForm!: FormGroup;
  transferForm!: FormGroup;
  priceForm!: FormGroup;

  contacts: Contact[] = [];
  readonly currencies = ['EUR', 'USD', 'GBP', 'CHF', 'AED', 'INR'];

  constructor(
    private propertyService: PropertyService,
    private contactService: ContactService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.buildForms();
    this.loadProperties();
    this.loadContacts();
  }

  buildForms(): void {
    this.propertyForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      address: ['', [Validators.required, Validators.maxLength(500)]],
      currentPrice: [null, [Validators.required, Validators.min(0)]],
      currency: ['EUR', Validators.required],
      dateOfRegistration: [new Date(), Validators.required]
    });

    this.transferForm = this.fb.group({
      contactId: ['', Validators.required],
      acquisitionPrice: [null, [Validators.required, Validators.min(0)]],
      currency: ['EUR', Validators.required]
    });

    this.priceForm = this.fb.group({
      newPrice: [null, [Validators.required, Validators.min(0)]],
      currency: ['EUR', Validators.required]
    });
  }

  loadProperties(): void {
    this.propertyService.getAll().subscribe({
      next: (res: any) => {
        this.dataSource.data = res.data?.items ?? res.data ?? [];
        this.dataSource.paginator = this.paginator;
      },
      error: () => this.notify('Failed to load properties.', true)
    });
  }

  loadContacts(): void {
    this.contactService.getAll().subscribe({
      next: (res: any) => {
        this.contacts = res.data?.items ?? res.data ?? [];
      }
    });
  }

  openAddPanel(): void {
    this.propertyForm.reset({ name: '', address: '', currentPrice: null, currency: 'EUR', dateOfRegistration: new Date() });
    this.selectedProperty = null;
    this.panelMode = 'add';
  }

  openEditPanel(row: Property): void {
    this.selectedProperty = row;
    this.propertyForm.patchValue({
      name: row.name,
      address: row.address,
      currentPrice: row.currentPrice,
      currency: 'EUR',
      dateOfRegistration: row.dateOfRegistration
    });
    this.panelMode = 'edit';
  }

  openTransferPanel(row: Property): void {
    this.selectedProperty = row;
    this.transferForm.reset({ contactId: '', acquisitionPrice: null, currency: 'EUR' });
    this.panelMode = 'transfer';
  }

  openPricePanel(row: Property): void {
    this.selectedProperty = row;
    this.priceForm.reset({ newPrice: null, currency: 'EUR' });
    this.panelMode = 'updatePrice';
  }

  closePanel(): void {
    this.panelMode = null;
    this.selectedProperty = null;
    this.isSubmitting = false;
  }

  submitPropertyForm(): void {
    if (this.propertyForm.invalid) { this.propertyForm.markAllAsTouched(); return; }
    this.isSubmitting = true;
    const val = this.propertyForm.value;

    if (this.panelMode === 'add') {
      const payload = { name: val.name, address: val.address, currentPrice: val.currentPrice, dateOfRegistration: new Date(val.dateOfRegistration).toISOString() };
      this.propertyService.create(payload as any).subscribe({
        next: () => { this.isSubmitting = false; this.closePanel(); this.notify('Property added successfully!'); this.loadProperties(); },
        error: (e: any) => { this.isSubmitting = false; this.notify(e?.error?.message ?? 'Failed to add property.', true); }
      });
    } else if (this.panelMode === 'edit' && this.selectedProperty) {
      const payload = { id: this.selectedProperty.id, name: val.name, address: val.address, currentPrice: val.currentPrice, dateOfRegistration: this.selectedProperty.dateOfRegistration };
      this.propertyService.update(this.selectedProperty.id, payload as any).subscribe({
        next: () => { this.isSubmitting = false; this.closePanel(); this.notify('Property updated successfully!'); this.loadProperties(); },
        error: (e: any) => { this.isSubmitting = false; this.notify(e?.error?.message ?? 'Failed to update property.', true); }
      });
    }
  }

  submitTransfer(): void {
    if (this.transferForm.invalid || !this.selectedProperty) { this.transferForm.markAllAsTouched(); return; }
    this.isSubmitting = true;
    const val = this.transferForm.value;
    this.propertyService.transfer(this.selectedProperty.id, { contactId: val.contactId, acquisitionPrice: val.acquisitionPrice, currency: val.currency }).subscribe({
      next: () => { this.isSubmitting = false; this.closePanel(); this.notify('Ownership transferred successfully!'); this.loadProperties(); },
      error: (e: any) => { this.isSubmitting = false; this.notify(e?.error?.message ?? 'Transfer failed.', true); }
    });
  }

  submitPriceUpdate(): void {
    if (this.priceForm.invalid || !this.selectedProperty) { this.priceForm.markAllAsTouched(); return; }
    this.isSubmitting = true;
    const val = this.priceForm.value;
    this.propertyService.updatePrice(this.selectedProperty.id, { newPrice: val.newPrice, currency: val.currency }).subscribe({
      next: () => { this.isSubmitting = false; this.closePanel(); this.notify('Price updated successfully!'); this.loadProperties(); },
      error: (e: any) => { this.isSubmitting = false; this.notify(e?.error?.message ?? 'Price update failed.', true); }
    });
  }

  deleteProperty(row: Property): void {
    if (!confirm(`Delete "${row.name}"? This will also remove its ownership and price history.`)) return;
    this.propertyService.delete(row.id).subscribe({
      next: () => { this.notify('Property deleted.'); this.loadProperties(); },
      error: (e: any) => this.notify(e?.error?.message ?? 'Failed to delete property.', true)
    });
  }

  applyFilter(event: Event): void {
    this.dataSource.filter = (event.target as HTMLInputElement).value.trim().toLowerCase();
  }

  private notify(msg: string, isError = false): void {
    this.snackBar.open(msg, 'Close', { duration: 3500, panelClass: isError ? ['snack-error'] : ['snack-success'] });
  }
}