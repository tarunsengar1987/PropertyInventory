import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { APIResponse } from '../model/api-response.model';
import {
  Property,
  CreatePropertyDto,
  UpdatePropertyDto,
  TransferOwnershipDto,
  UpdatePriceDto
} from '../model/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {

  private baseUrl = `${environment.apiUrl}Property`;

  constructor(private http: HttpClient) { }

  getAll(search?: string): Observable<APIResponse<Property[]>> {
    const params: any = {};
    if (search) params['search'] = search;
    return this.http.get<APIResponse<Property[]>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<APIResponse<Property>> {
    return this.http.get<APIResponse<Property>>(`${this.baseUrl}/${id}`);
  }

  create(data: CreatePropertyDto): Observable<APIResponse<Property>> {
    return this.http.post<APIResponse<Property>>(this.baseUrl, data);
  }

  update(id: string, data: UpdatePropertyDto): Observable<APIResponse<Property>> {
    return this.http.put<APIResponse<Property>>(`${this.baseUrl}/${id}`, data);
  }

  delete(id: string): Observable<APIResponse<void>> {
    return this.http.delete<APIResponse<void>>(`${this.baseUrl}/${id}`);
  }

  transfer(propertyId: string, data: TransferOwnershipDto): Observable<APIResponse<void>> {
    return this.http.post<APIResponse<void>>(`${this.baseUrl}/${propertyId}/transfer`, data);
  }

  updatePrice(propertyId: string, data: UpdatePriceDto): Observable<APIResponse<void>> {
    return this.http.post<APIResponse<void>>(`${this.baseUrl}/${propertyId}/update-price`, data);
  }
}
