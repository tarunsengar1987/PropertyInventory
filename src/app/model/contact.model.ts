/**
 * Contact model representing a property owner or contact person.
 */
export interface Contact {
  id: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
}

/**
 * DTO used when creating a new contact.
 */
export interface CreateContactDto {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
}

/**
 * DTO used when updating an existing contact.
 */
export interface UpdateContactDto {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
}
