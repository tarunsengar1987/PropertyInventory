export interface Property {
  id: string;
  name: string;
  address: string;
  currentPrice: number;
  dateOfRegistration: string;
}

export interface CreatePropertyDto {
  name: string;
  address: string;
  currentPrice: number;
  dateOfRegistration: string;
}

export interface UpdatePropertyDto {
  id: string;
  name: string;
  address: string;
  currentPrice: number;
  dateOfRegistration: string;
}

export interface TransferOwnershipDto {
  contactId: string;
  acquisitionPrice: number;
  currency: string;
}

export interface UpdatePriceDto {
  newPrice: number;
  currency: string;
}
