import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Contact } from 'src/app/model/contact.model';
import { ContactService } from 'src/app/service/contact.service';
import { NotificationService } from 'src/app/service/notification.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.scss']
})
export class ContactsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['firstName', 'lastName', 'phoneNumber', 'email', 'actions'];

  dataSource = new MatTableDataSource<Contact>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  contactForm!: FormGroup;

  showForm = false;
  editingId: string | null = null;

  constructor(
    private contactService: ContactService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) { }

  ngOnInit(): void {
    this.buildForm();
    this.loadContacts();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  private buildForm(): void {
    this.contactForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      phoneNumber: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  loadContacts(): void {
    this.contactService.getAll().subscribe({
      next: (res) => {
        this.dataSource.data = res.data.items;
      }
    });
  }

  applyFilter(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.dataSource.filter = value.trim().toLowerCase();
  }

  openAddForm(): void {
    this.editingId = null;
    this.contactForm.reset();
    this.showForm = true;
  }

  openEditForm(contact: Contact): void {
    this.editingId = contact.id;

    this.contactForm.patchValue({
      firstName: contact.firstName,
      lastName: contact.lastName,
      phoneNumber: contact.phoneNumber,
      email: contact.email
    });

    this.showForm = true;
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingId = null;
    this.contactForm.reset();
  }

  submitForm(): void {
    if (this.contactForm.invalid) return;

    if (this.editingId) {
      const payload = { id: this.editingId, ...this.contactForm.value };
      this.contactService.update(this.editingId, payload).subscribe({
        next: () => {
          this.notificationService.success('Contact updated successfully.');
          this.cancelForm();
          this.loadContacts();
        },
        error: () => {
          this.notificationService.error('Failed to update contact.');
        }
      });
    } else {
      const payload = this.contactForm.value;
      this.contactService.create(payload).subscribe({
        next: () => {
          this.notificationService.success('Contact created successfully.');
          this.cancelForm();
          this.loadContacts();
        },
        error: () => {
          this.notificationService.error('Failed to create contact.');
        }
      });
    }
  }


  deleteContact(contact: Contact): void {
    if (!confirm(`Delete ${contact.firstName} ${contact.lastName}? This cannot be undone.`)) return;
    this.contactService.delete(contact.id).subscribe({
      next: () => {
        this.notificationService.success('Contact deleted successfully.');
        this.loadContacts();
      },
      error: () => this.notificationService.error('Failed to delete contact.')
    });
  }

  get isEditing(): boolean {
    return this.editingId !== null;
  }
}